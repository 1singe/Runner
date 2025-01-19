using System.Collections.Generic;
using PGG;
using ProcGen;
using Unity.VisualScripting;
using UnityEngine;


public class ProceduralGenerationManager : MonoBehaviour
{
    public Chunk ChunkPrefab;

    //public bool UseAsset = false;
    //public NoiseData[] Noises;

    public GraphAsset GenerationAsset;

    public int ChunkSize = 241;

    public Vector2 Offset;

    [Range(0, 6)] public int LOD = 1;

    public GameObject ChunksHolder;

    public bool UseDefinedSeed = true;
    public int PredefinedSeed;

    public bool AutoUpdateOnValidate = false;

    [HideInInspector] public int internSeed { get; private set; }

    // Editor 

    [InspectorLabel("Testing")] private Chunk EditorChunk;
    public Material RenderMaterial;

    public static ProceduralGenerationManager Instance { get; private set; }


    public ProceduralGenerationManager() : base()
    {
        if (!Instance)
            Instance = this;
    }


    public static float SampleHeightAtPos(Vector2 worldPos)
    {
        return Generated_GenerationStatics.SampleDunes(worldPos.x, worldPos.y); //Instance.GenerationAsset.SampleGraphAtPos(worldPos.x, worldPos.y);
    }

    public void Editor_GenerateSingleChunk(int seed)
    {
        Editor_DestroyChunk();

        EditorChunk = Instantiate(ChunkPrefab, Vector2.zero, Quaternion.identity, ChunksHolder.transform);

        EditorChunk.CreateMeshAndOverrideMaterial(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(ChunkSize, GenerationAsset, seed, Offset, Vector2.zero), LOD), RenderMaterial);
    }

    //public void Editor_GenerateSingleChunk(int seed)
    //{
    //    Editor_DestroyChunk();
    //
    //    EditorChunk = Instantiate(ChunkPrefab, Vector2.zero, Quaternion.identity, ChunksHolder.transform);
    //
    //    EditorChunk.CreateMesh(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(ChunkSize, Noises, internSeed, Offset, Vector2.zero), LOD));
    //}

    public void Editor_DestroyChunk()
    {
        if (EditorChunk)
        {
            DestroyImmediate(EditorChunk.gameObject);
        }
    }

    public void GenerateChunk(Vector2 chunkPos)
    {
        Chunk currentChunk = Instantiate(ChunkPrefab, chunkPos, Quaternion.identity, ChunksHolder.transform);
        currentChunk.CreateMesh(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(ChunkSize, GenerationAsset, internSeed, Offset, Vector2.zero), LOD));
    }

    private void OnValidate()
    {
        if (!AutoUpdateOnValidate)
            return;

        Regenerate();
    }

    public void Regenerate()
    {
        RuntimeTerrainGenerator RuntimeGenerator = RuntimeTerrainGenerator.Instance;
        if (RuntimeGenerator)
        {
            CookGraph(internSeed);
            foreach (KeyValuePair<Vector2Int, RuntimeTerrainGenerator.ChunkData> ChunkData in RuntimeGenerator.ChunksData)
            {
                ChunkData.Value.Chunk.CreateMesh(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(ChunkSize, GenerationAsset, internSeed, Offset, Vector2.zero), LOD));
            }
        }
    }

    void Awake()
    {
        if (!Instance)
            Instance = this;

        if (UseDefinedSeed)
        {
            internSeed = PredefinedSeed;
        }
        else
        {
            internSeed = (int)System.DateTime.Now.Ticks;
        }

        Random.InitState(internSeed);

        CookGraph(internSeed);
    }

    public void CookGraph(int seed)
    {
        GenerationAsset.Cook();
    }


    public void DEP_InitRandomGenerator(NoiseData[] noises, int seed)
    {
        GenerationAsset.Cook();

        int SeedCrypterIterator = 2;

        // Initialize every noise object
        foreach (NoiseData Noise in noises)
        {
            Noise.NoiseInstance = new FastNoise();
            Noise.Seed = seed * SeedCrypterIterator;

            if (Noise.UpdateSeed)
            {
                SeedCrypterIterator *= 2;
            }

            Noise.NoiseInstance.SetNoiseType(Noise.NoiseType);
            Noise.NoiseInstance.SetFrequency(Noise.Frequency);
            Noise.NoiseInstance.SetSeed(Noise.Seed);
            Noise.NoiseInstance.SetFractalType(Noise.FractalType);
            Noise.NoiseInstance.SetFractalLacunarity(Noise.FractalLacunarity);
            Noise.NoiseInstance.SetFractalGain(Noise.FractalGain);
            Noise.NoiseInstance.SetFractalOctaves(Noise.FractalOctaves);
        }
    }
}


public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] heightMap, int LOD)
    {
        int simplification = LOD <= 0 ? 1 : LOD * 2;

        int chunkSize = ProceduralGenerationManager.Instance.ChunkSize;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int verticesPerLine = (width - 1) / simplification + 1;

        float topLeftX = 0; //(chunkSize - 1) / -2f;
        float topLeftZ = 0; //(chunkSize - 1) / 2f;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < chunkSize; y += simplification)
        {
            for (int x = 0; x < chunkSize; x += simplification)
            {
                meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ + y);
                meshData.UVs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < chunkSize - 1 && y < chunkSize - 1)
                {
                    meshData.CreateTriangle(vertexIndex, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
                    meshData.CreateTriangle(vertexIndex + verticesPerLine + 1, vertexIndex + 1, vertexIndex);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}