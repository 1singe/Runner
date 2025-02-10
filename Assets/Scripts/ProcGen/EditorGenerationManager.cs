using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGG;
using ProcGen;
using ProcGen.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


[ExecuteInEditMode]
public class EditorGenerationManager : MonoBehaviour
{
    [HideInInspector] public bool Attached = false;

    [SerializeField] private GraphAsset _graphAsset;
    public GraphAsset GraphAsset => _graphAsset;

    [SerializeField] private int _generationHalfSize = 3;
    public int GenerationHalfSize => _generationHalfSize;

    [SerializeField] private int _chunkSize = 241;
    public int ChunkSize => _chunkSize;

    [SerializeField] private Chunk _chunkPrefab;
    public Chunk ChunkPrefab => _chunkPrefab;

    private Dictionary<int2, Chunk> _chunkDictionary;
    private Dictionary<int2, Task<MeshData>> _meshDataTasks;
    private ChunksContainer _chunkContainer;
    [SerializeField] private Material _heightMaterial;
    private static readonly int MinMax = Shader.PropertyToID("_MinMax");

    public void OnEnable()
    {
        _chunkContainer = FindFirstObjectByType<ChunksContainer>();
        OnAttached();
    }

    public Bounds GetBounds()
    {
        float halfChunkSize = (ChunkSize - 1f) / 2f;
        return new Bounds(new Vector3(halfChunkSize, 0f, halfChunkSize), Vector3.one * 2 * (_generationHalfSize + 1) * ChunkSize);
    }

    public void OnDisable()
    {
        OnDetached();
    }

    public void OnAttached()
    {
        _chunkDictionary = new Dictionary<int2, Chunk>();
        _chunkContainer = FindFirstObjectByType<ChunksContainer>();
        _meshDataTasks = new Dictionary<int2, Task<MeshData>>();
        _graphAsset.OnGraphCooked.AddListener(OnGraphCooked);
    }

    public void Awake()
    {
        CleanChunks();
    }

    private void CleanChunks()
    {
        for (int i = _chunkContainer.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_chunkContainer.transform.GetChild(i).gameObject);
        }

        _chunkDictionary.Clear();
        _meshDataTasks.Clear();
    }

    public void OnDetached()
    {
        _graphAsset.OnGraphCooked.RemoveListener(OnGraphCooked);
        if (_chunkDictionary != null)
        {
            foreach (var (pos, chunk) in _chunkDictionary)
            {
                DestroyImmediate(chunk.gameObject);
            }

            _chunkDictionary.Clear();
        }
        else
        {
            CleanChunks();
        }
    }

    private async void OnGraphCooked()
    {
        await Generate();
    }

    private async Task Generate()
    {
        float min = 5000f;
        float max = -5000f;

        CleanChunks();
        for (int y = -_generationHalfSize; y <= _generationHalfSize; y++)
        {
            for (int x = -_generationHalfSize; x <= _generationHalfSize; x++)
            {
                int2 chunkCoords = new int2(x, y);
                float3 chunkWorldPos = new float3(chunkCoords.x * (_chunkSize - 1f), 0f, chunkCoords.y * (_chunkSize - 1f));

                _meshDataTasks[chunkCoords] = Task.Run(() =>
                {
                    MinMaxHeightMap minMaxHeightMap = GraphAsset.ProcessGraph(GraphAsset.OutputNode, (_chunkSize), chunkWorldPos.x, chunkWorldPos.z);
                    MeshData meshData = MeshGenerator.GenerateMesh(minMaxHeightMap.HeightMap, 0, ChunkSize);
                    min = Mathf.Min(min, minMaxHeightMap.Min);
                    max = Mathf.Max(max, minMaxHeightMap.Max);
                    return meshData;
                });
            }
        }

        for (int y = -_generationHalfSize; y <= _generationHalfSize; y++)
        {
            for (int x = -_generationHalfSize; x <= _generationHalfSize; x++)
            {
                int2 chunkCoords = new int2(x, y);
                float3 chunkWorldPos = new float3(chunkCoords.x * (_chunkSize - 1f), 0f, chunkCoords.y * (_chunkSize - 1f));

                await Task.WhenAll(_meshDataTasks[chunkCoords]);

                Mesh mesh = _meshDataTasks[chunkCoords].Result.CreateMesh();
                Chunk chunk;

                if (_chunkDictionary.TryGetValue(chunkCoords, out var outChunk))
                {
                    chunk = outChunk;
                }
                else
                {
                    chunk = Instantiate(_chunkPrefab, new float3(chunkWorldPos.x, 0f, chunkWorldPos.z), Quaternion.identity, _chunkContainer.transform);
                    _chunkDictionary.Add(chunkCoords, chunk);
                }

                chunk.MeshFilter.mesh = mesh;
            }
        }

        Paint(min, max);
    }

    private void Paint(float min, float max)
    {
        Material mat = new Material(_heightMaterial);
        mat.SetVector(MinMax, new Vector4(min, max));
        foreach (var (pos, chunk) in _chunkDictionary)
        {
            chunk.MeshRenderer.material = mat;
        }
    }
}