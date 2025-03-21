using Unity.Mathematics;
using UnityEngine;


public class BiomeLookup
{

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
}