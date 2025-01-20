using System;
using System.Collections.Generic;
using PGG;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

namespace ProcGen.Jobs
{
    public class ParallelGenerator : MonoBehaviour
    {
        public struct ChunkData
        {
            public Vector3 ChunkWorldPos;
            public JobHandle Handle;
            public NativeMeshData MeshData;
        }

        public ProceduralGenerationManager Manager;
        private Dictionary<Vector2Int, ChunkData> _chunkDataDictionary;

        public void Awake()
        {
            Manager = GetComponent<ProceduralGenerationManager>();
            _chunkDataDictionary = new Dictionary<Vector2Int, ChunkData>();
        }

        public void GenerateChunk(Vector2Int chunkCoords, Vector3 position3D, int LOD)
        {
            ChunkData data = new ChunkData()
            {
                ChunkWorldPos = position3D,
                MeshData = new NativeMeshData(Manager.ChunkSize, LOD, Allocator.TempJob)
            };

            ComputeHeightJob job = new ComputeHeightJob
            {
                MeshData = data.MeshData,
                ChunkWorldPos = new float2 { x = position3D.x, y = position3D.z }
            };

            data.Handle = job.Schedule(data.MeshData.Vertices.Length, 64);

            _chunkDataDictionary.Add(chunkCoords, data);
        }

        public void LateUpdate()
        {
            foreach (KeyValuePair<Vector2Int, ChunkData> data in _chunkDataDictionary)
            {
                JobHandle handle = data.Value.Handle;
                if (!handle.IsCompleted)
                    continue;

                handle.Complete();
                Manager.OnMapDataReceived(data.Value);
                data.Value.MeshData.Dispose();
            }
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
    }

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

        public void CreateTriangle(int index, int a, int b, int c)
        {
            Triangles[index] = a;
            Triangles[index + 1] = b;
            Triangles[index + 2] = c;
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
}