using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/Procedural/Biome", order = 1)]
public class Biome : ScriptableObject
{
    public NoiseData[] Noises = new NoiseData[] { };

    public Biome()
    {
        int SeedCrypterIterator = 2;

        // Initialize every noise object
        foreach (NoiseData Noise in Noises)
        {
            Noise.NoiseInstance = new FastNoise();
            Noise.Seed = 1337 * SeedCrypterIterator;
            SeedCrypterIterator *= 2;

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