using System;
using Unity.Mathematics;
using UnityEngine;

public struct BiomeWeights
{
    public static int BiomeNb = 4;
    public static readonly float3 Dunes = new float3(1f, 0.5f, 0f);
    public static readonly float3 Tundra = new float3(0f, 0.6f, 0.2f);
    public static readonly float3 Ocean = new float3(0f, 0f, 1f);
    public static readonly float3 Mountains = new float3(0f, 1f, 0.2f);
    
    public static readonly float3[] Biomes = { Dunes, Tundra, Ocean, Mountains };
    
    public float[] BiomeWeight;

    public BiomeWeights(float[] weights)
    {
        BiomeWeight = new float[BiomeNb];
        for (int i = 0; i < weights.Length; i++)
        {
            BiomeWeight[i] = weights[i];
        }
    }
}

public class BiomeLookup
{
    
    //Config
    private static readonly float BiomeSmoothing = 0.02f;
    private static readonly float RelevancyThreshold = 0.05f;

    private static readonly int BiomeLookupTableDimensions = 256;

    private static readonly BiomeWeights[,,] BiomeLookupTable = new BiomeWeights[BiomeLookupTableDimensions, BiomeLookupTableDimensions, BiomeLookupTableDimensions];
    
   // private static readonly float3 Dunes = new float3(1f, 0f, 0f);
   // private static readonly float3 Tundra = new float3(0f, 1f, 0.2f);
   // private static readonly float3 Other = new float3(0f, 1f, 1f);

    //private static readonly float3[] Biomes = { Dunes, Tundra, Other };

    private static readonly FastNoise _temperatureNoise = InitTemperatureNoise();

    private static FastNoise InitTemperatureNoise()
    {
        FastNoise temperatureNoise = new FastNoise();
        temperatureNoise.SetFrequency(0.005f);
        temperatureNoise.SetSeed(1337);
        temperatureNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
        return temperatureNoise;
    }

    private static readonly FastNoise _elevationNoise = InitElevationNoise();

    private static FastNoise InitElevationNoise()
    {
        FastNoise elevationNoise = new FastNoise();
        elevationNoise.SetFrequency(0.005f);
        elevationNoise.SetSeed(1337);
        elevationNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
        elevationNoise.SetFractalType(FastNoise.FractalType.FBM);
        elevationNoise.SetFractalGain(-0.57f);
        elevationNoise.SetFractalLacunarity(1.28f);
        elevationNoise.SetFractalOctaves(3);
        elevationNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);

        return elevationNoise;
    }

    private static readonly FastNoise _humidityNoise = InitHumidityNoise();

    private static FastNoise InitHumidityNoise()
    {
        FastNoise humidityNoise = new FastNoise();
        humidityNoise.SetFrequency(0.005f);
        humidityNoise.SetSeed(1337);
        humidityNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
        return humidityNoise;
    }


    float3 GetCaracteristics(float x, float y)
    {
        return new float3(_humidityNoise.GetNoise(x, y), _elevationNoise.GetNoise(x, y), _temperatureNoise.GetNoise(x, y));
    }

    void PopulateBiomeLookupTable()
    {
        
        for (int x = 0; x < BiomeLookupTableDimensions; x++)
        {
            for(int y = 0; y < BiomeLookupTableDimensions; y++)
            {
                for(int z = 0; z < BiomeLookupTableDimensions; z++)
                {
                    float3 pt = new float3(x / (float)BiomeLookupTableDimensions, y / (float)BiomeLookupTableDimensions, z / (float)BiomeLookupTableDimensions);
                    float[] distances = new float[BiomeWeights.BiomeNb];
                    int min = -1;
                    float minDist = float.MaxValue;
                    for (int i = 0; i < BiomeWeights.BiomeNb; i++)
                    {
                        distances[i] = math.distancesq(pt, BiomeWeights.Biomes[i]);
                        if (distances[i] < minDist)
                        {
                            minDist = distances[i];
                            min = i;
                        }
                    }
                    float[] weights = new float[BiomeWeights.BiomeNb];
                    for (int i = 0; i < BiomeWeights.BiomeNb; i++)
                    {
                        weights[i] = distances[i];
                    }
                }
            }
        }
        for (int i = 0; i < BiomeWeights.BiomeNb; i++)
        {
            
        }
    }
    
    float[] GetBiomeWeight(float x, float y)
    {
        float3 caracteristics = GetCaracteristics(x, y);
        
        int BiomeCellX = Mathf.RoundToInt(caracteristics.x * BiomeLookupTableDimensions);
        int BiomeCellY = Mathf.RoundToInt(caracteristics.y * BiomeLookupTableDimensions);
        int BiomeCellZ = Mathf.RoundToInt(caracteristics.z * BiomeLookupTableDimensions);

        return null;
    }
}