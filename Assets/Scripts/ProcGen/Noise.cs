using System;
using PGG;
using UnityEngine;
using Random = UnityEngine.Random;

public enum NoiseMixMode
{
    Additive,
    Multiplicative,
    Subtractive,
    Inverse,
}

[Serializable]
public class NoiseData
{
    public string Name = "None";
    public float Amplitude = 1f;

    [Range(0f, 1f)] public float Frequency = 1f;

    public NoiseMixMode Mode = NoiseMixMode.Multiplicative;
    public FastNoise.NoiseType NoiseType = FastNoise.NoiseType.Perlin;

    public FastNoise.FractalType FractalType = FastNoise.FractalType.FBM;
    public float FractalGain = 0.5f;
    public float FractalLacunarity = 2f;
    public int FractalOctaves = 3;

    public FastNoise NoiseInstance;

    public Vector2 randomOffset { get; private set; }
    [HideInInspector] public int Seed;

    public bool UpdateSeed = false;
}


public static class Noise
{
    public static float ApplyNoiseFilterOnPoint(float input, float sample, float amplitude, NoiseMixMode mode)
    {
        float ampSample = sample * amplitude;

        switch (mode)
        {
            case NoiseMixMode.Additive:
                return input + ampSample;
            case NoiseMixMode.Multiplicative:
                return input * ampSample;
            case NoiseMixMode.Subtractive:
                return input - ampSample;
            case NoiseMixMode.Inverse:
                return input * 1f / ampSample;
            default:
                return 0f;
        }
    }

    public static float SampleHeightAtPos(ProceduralGenerationManager Manager, Vector2 worldPos, NoiseData[] noises)
    {
        float finalValue = 1f;

        for (int i = 0; i < noises.Length; i++)
        {
            NoiseData currentData = noises[i];

            float perlinValue = currentData.NoiseInstance.GetNoise(worldPos.x + Manager.Offset.x, worldPos.y + Manager.Offset.y);

            finalValue = ApplyNoiseFilterOnPoint(finalValue, perlinValue, currentData.Amplitude, currentData.Mode);
        }

        return finalValue;
    }

    public static float SampleHeightAtPos(Vector2 worldPos, NoiseData[] noises)
    {
        return SampleHeightAtPos(ProceduralGenerationManager.Instance, worldPos, noises);
    }

    public static float SampleHeightAtPos(ProceduralGenerationManager manager, Vector2 worldPos, GraphAsset generationAsset)
    {
        return generationAsset.SampleGraphAtPos(worldPos.x + manager.Offset.x, worldPos.y + manager.Offset.y);
    }

    public static float SampleHeightAtPos(Vector2 worldPos, GraphAsset generationAsset)
    {
        return SampleHeightAtPos(ProceduralGenerationManager.Instance, worldPos, generationAsset);
    }

    public static float[,] GenerateNoiseMap(int chunkSize, NoiseData[] noises, int masterSeed, Vector2 offset, Vector2 chunkPos)
    {
        Random.InitState(masterSeed);

        ProceduralGenerationManager ProceduralManager = ProceduralGenerationManager.Instance;
        bool FoundManager = ProceduralManager;

        float[,] noiseMap = new float [chunkSize, chunkSize];

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                float sampleX = (x + chunkPos.x + offset.x);
                float sampleY = (y + chunkPos.y + offset.y);

                if (FoundManager)
                {
                    noiseMap[x, y] = SampleHeightAtPos(ProceduralManager, new Vector2(sampleX, sampleY), noises);
                }
                else
                {
                    noiseMap[x, y] = SampleHeightAtPos(new Vector2(sampleX, sampleY), noises);
                }
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateNoiseMap(int chunkSize, GraphAsset generationAsset, int masterSeed, Vector2 offset, Vector2 chunkPos)
    {
        Random.InitState(masterSeed);

        ProceduralGenerationManager ProceduralManager = ProceduralGenerationManager.Instance;
        bool FoundManager = ProceduralManager;

        generationAsset.Cook();
        float[,] noiseMap = new float [chunkSize, chunkSize];

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                float sampleX = (x + chunkPos.x + offset.x);
                float sampleY = (y + chunkPos.y + offset.y);

                if (FoundManager)
                {
                    noiseMap[x, y] = SampleHeightAtPos(ProceduralManager, new Vector2(sampleX, sampleY), generationAsset);
                }
                else
                {
                    noiseMap[x, y] = SampleHeightAtPos(new Vector2(sampleX, sampleY), generationAsset);
                }
            }
        }

        return noiseMap;
    }
}

public struct MeshData
{
    public Vector3[] Vertices;
    public int[] Triangles;
    public Vector2[] UVs;

    int _currentTriangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        Vertices = new Vector3[meshWidth * meshHeight];
        UVs = new Vector2[meshWidth * meshHeight];
        Triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        _currentTriangleIndex = 0;
    }

    public void CreateTriangle(int a, int b, int c)
    {
        Triangles[_currentTriangleIndex++] = a;
        Triangles[_currentTriangleIndex++] = b;
        Triangles[_currentTriangleIndex++] = c;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = UVs;

        mesh.RecalculateNormals();

        return mesh;
    }
}