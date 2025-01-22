using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PGG;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace ProcGen.Jobs
{
    public struct NativeMeshData : IDisposable
    {
        [ReadOnly] public int Size;
        [ReadOnly] public int Simplification;
        [ReadOnly] public int VerticesByLine;
        public NativeArray<float3> Vertices;
        public NativeArray<float2> UVs;
        [NativeDisableParallelForRestriction] public NativeArray<int> Triangles;

        public NativeMeshData(int size, int LOD, Allocator allocator)
        {
            Size = size;
            Simplification = LOD <= 0 ? 1 : LOD * 2;
            VerticesByLine = (Size - 1) / Simplification + 1;

            Vertices = new NativeArray<float3>(VerticesByLine * VerticesByLine, allocator);
            UVs = new NativeArray<float2>(VerticesByLine * VerticesByLine, allocator);
            Triangles = new NativeArray<int>((VerticesByLine - 1) * (VerticesByLine - 1) * 6, allocator);
        }

        public void Dispose()
        {
            Vertices.Dispose();
            UVs.Dispose();
            Triangles.Dispose();
        }

        public int CreateTriangle(int index, int a, int b, int c)
        {
            Triangles[index++] = a;
            Triangles[index++] = b;
            Triangles[index++] = c;

            return index;
        }

        public void CreateQuad(int index, int a, int b, int c, int d)
        {
            Triangles[index] = a;
            Triangles[index + 1] = b;
            Triangles[index + 2] = c;
            Triangles[index + 3] = a;
            Triangles[index + 4] = c;
            Triangles[index + 5] = d;
        }
    }

    public struct ChunkData
    {
        public bool IsGenerated;
        public float3 ChunkWorldPos;
        public JobHandle Handle;
        public NativeMeshData MeshData;
        public Bounds Bounds;
        public Chunk Chunk;
    }

    // Async and multi-threaded version of previous generator that utilizes Unity.Mathematics types
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Assets")] [SerializeField] private Chunk _chunkPrefab;
        public Chunk ChunkPrefab => _chunkPrefab;

        [SerializeField] private GraphAsset _generationGraph;
        public GraphAsset GenerationGraph => _generationGraph;

        [Header("Parameters")] [SerializeField]
        private int _chunkSize = 241;

        public int ChunkSize => _chunkSize;

        [SerializeField] private float _viewDistance = 1000f;
        public float ViewDistance => _viewDistance;


        private Camera _mainCamera;
        private float3 _viewPosition;
        private int _visibleChunks;
        private ChunksContainer _chunksContainer;
        private Dictionary<int2, ChunkData> _generationJobContainers;
        private List<ChunkData> _chunksVisibleLastFrame;

        private void Awake()
        {
            // Init
            _chunksContainer = FindFirstObjectByType<ChunksContainer>();
            _generationJobContainers = new Dictionary<int2, ChunkData>();
            _chunksVisibleLastFrame = new List<ChunkData>();
            _mainCamera = Camera.main;

            // Validation
            Assert.IsNotNull(_mainCamera);
            Assert.IsNotNull(ChunkPrefab);
            Assert.IsNotNull(GenerationGraph);
            Assert.IsNotNull(_chunksContainer);
        }

        private async void Start()
        {
            while (true)
            {
                UpdateChunksVisibility();
                await Awaitable.NextFrameAsync();
            }
        }

        private void UpdateChunksVisibility()
        {
            for (int i = 0; i < _chunksVisibleLastFrame.Count; i++)
            {
                SetVisibility(_chunksVisibleLastFrame[i], false);
            }

            _chunksVisibleLastFrame.Clear();

            int viewChunkPosX = Mathf.RoundToInt(_viewPosition.x / (_chunkSize));
            int viewChunkPosY = Mathf.RoundToInt(_viewPosition.z / (_chunkSize));

            for (int y = -_visibleChunks; y <= _visibleChunks; y++)
            {
                for (int x = -_visibleChunks; x <= _visibleChunks; x++)
                {
                    int2 chunkCoords = new int2(viewChunkPosX + x, viewChunkPosY + y);

                    if (!_generationJobContainers.ContainsKey(chunkCoords) || !_generationJobContainers[chunkCoords].IsGenerated)
                    {
                        continue;
                    }

                    UpdateVisibility(_generationJobContainers[chunkCoords]);
                    if (IsVisible(_generationJobContainers[chunkCoords]))
                    {
                        _chunksVisibleLastFrame.Add(_generationJobContainers[chunkCoords]);
                    }
                }
            }
        }

        private async void Update()
        {
            _viewPosition = _mainCamera.transform.position;
            _visibleChunks = Mathf.RoundToInt(ViewDistance / (ChunkSize - 1));
            await GenerateVisibleChunks();
        }

        private void UpdateVisibility(ChunkData data)
        {
            float viewDistanceFromBounds = Mathf.Sqrt(data.Bounds.SqrDistance(_viewPosition));
            SetVisibility(data, viewDistanceFromBounds <= _viewDistance);
        }

        private void SetVisibility(ChunkData data, bool visible)
        {
            data.Chunk.gameObject.SetActive(visible);
        }

        private bool IsVisible(ChunkData data)
        {
            return data.Chunk.gameObject.activeSelf;
        }

        private async Task GenerateVisibleChunks()
        {
            int viewChunkPosX = Mathf.RoundToInt(_viewPosition.x / (_chunkSize));
            int viewChunkPosY = Mathf.RoundToInt(_viewPosition.z / (_chunkSize));

            for (int y = -_visibleChunks; y <= _visibleChunks; y++)
            {
                for (int x = -_visibleChunks; x <= _visibleChunks; x++)
                {
                    int2 chunkCoords = new int2(viewChunkPosX + x, viewChunkPosY + y);

                    if (!_generationJobContainers.ContainsKey(chunkCoords))
                    {
                        _generationJobContainers.Add(chunkCoords, new ChunkData());
                        await RequestChunkGeneration(chunkCoords, 0);
                    }
                }
            }
        }


        public static float GetHeight_World(float2 worldPos)
        {
            return Generated_GenerationStatics.SampleDunes(worldPos.x, worldPos.y);
        }

        private async Task RequestChunkGeneration(int2 chunkCoords, int LOD)
        {
            float3 chunkWorldPos = new float3 { x = chunkCoords.x * (_chunkSize - 1f), y = 0f, z = chunkCoords.y * (_chunkSize - 1f) };
            await GenerateChunk(chunkCoords, chunkWorldPos, LOD);
        }


        private async Task GenerateChunk(int2 chunkCoords, float3 chunkWorldPos, int LOD)
        {
            ChunkData data = new ChunkData()
            {
                MeshData = new NativeMeshData(_chunkSize, LOD, Allocator.TempJob),
                ChunkWorldPos = chunkWorldPos
            };

            ComputeMeshHeightMap job = new ComputeMeshHeightMap
            {
                MeshData = data.MeshData,
                ChunkWorldPos = new float2 { x = chunkWorldPos.x, y = chunkWorldPos.z }
            };

            data.Handle = job.ScheduleByRef();

            while (!data.Handle.IsCompleted)
                await Task.Yield();

            data.Handle.Complete();

            Chunk chunk = Instantiate(ChunkPrefab, chunkWorldPos, Quaternion.identity, _chunksContainer.transform);
            Mesh mesh = new Mesh();
            mesh.vertices = data.MeshData.Vertices.Reinterpret<Vector3>(UnsafeUtility.SizeOf<Vector3>()).ToArray();
            mesh.triangles = data.MeshData.Triangles.Reinterpret<int>().ToArray();
            mesh.uv = data.MeshData.UVs.Reinterpret<Vector2>(UnsafeUtility.SizeOf<Vector2>()).ToArray();
            mesh.RecalculateNormals();
            chunk.MeshFilter.sharedMesh = mesh;
            data.Chunk = chunk;
            data.IsGenerated = true;
            data.Bounds = new Bounds(chunkWorldPos + new float3((_chunkSize - 1f) / 2f, (_chunkSize - 1f) / 2f, _chunkSize / 2f), Vector2.one * (_chunkSize - 1f));
            _generationJobContainers[chunkCoords] = data;
            data.MeshData.Dispose();
        }


        [BurstCompile]
        public struct ComputeHeightJob : IJobParallelFor
        {
            [NativeDisableParallelForRestriction] public NativeMeshData MeshData;
            [ReadOnly] public float2 ChunkWorldPos;

            public void Execute(int index)
            {
                int x = index % MeshData.VerticesByLine;
                int y = index / MeshData.VerticesByLine;

                if (x % MeshData.Simplification != 0 && y % MeshData.Simplification != 0)
                    return;

                float xPos = ChunkWorldPos.x + x;
                float zPos = ChunkWorldPos.y + y;

                MeshData.Vertices[index] = new float3
                {
                    x = x,
                    y = Generated_GenerationStatics.SampleDunes(xPos, zPos),
                    z = y
                };

                MeshData.UVs[index] = new float2
                {
                    x = x / (float)MeshData.Size,
                    y = y / (float)MeshData.Size,
                };

                if (x < MeshData.Size - 1 && y < MeshData.Size - 1)
                {
                    MeshData.CreateQuad(index * 6, index, index + MeshData.VerticesByLine, index + MeshData.VerticesByLine + 1, index + 1);
                }
            }
        }

        [BurstCompile]
        public struct ComputeMeshHeightMap : IJob
        {
            public NativeMeshData MeshData;
            [ReadOnly] public float2 ChunkWorldPos;

            public void Execute()
            {
                int quadIndex = 0;
                int vertexIndex = 0;

                for (int y = 0; y < MeshData.Size; y += MeshData.Simplification)
                {
                    for (int x = 0; x < MeshData.Size; x += MeshData.Simplification)
                    {
                        MeshData.Vertices[quadIndex] = new float3(x, Generated_GenerationStatics.SampleDunes(x + ChunkWorldPos.x, y + ChunkWorldPos.y), y);
                        MeshData.UVs[quadIndex] = new float2(x / (float)MeshData.Size, y / (float)MeshData.Size);

                        // Todo maybe calculate normals here ?

                        if (x < MeshData.Size - 1 && y < MeshData.Size - 1)
                        {
                            vertexIndex = MeshData.CreateTriangle(vertexIndex, quadIndex, quadIndex + MeshData.VerticesByLine, quadIndex + MeshData.VerticesByLine + 1);
                            vertexIndex = MeshData.CreateTriangle(vertexIndex, quadIndex + MeshData.VerticesByLine + 1, quadIndex + 1, quadIndex);
                        }

                        quadIndex++;
                    }
                }
            }
        }
    }
}