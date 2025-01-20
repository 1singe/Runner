public partial struct Generated_GenerationStatics
{
    public static float SampleDunes(float x, float y)
    {
        SFastNoise noise89b3fd8b_a932_42ba_be4f_f7e1bb820182 = new SFastNoise();
        noise89b3fd8b_a932_42ba_be4f_f7e1bb820182.SetFrequency(0.015f);
        noise89b3fd8b_a932_42ba_be4f_f7e1bb820182.SetSeed(1337);
        noise89b3fd8b_a932_42ba_be4f_f7e1bb820182.SetNoiseType(SFastNoise.NoiseType.Perlin);
        SFastNoise noiseda87bcd4_1563_446d_a9bf_9a85382e6752 = new SFastNoise();
        noiseda87bcd4_1563_446d_a9bf_9a85382e6752.SetFrequency(0.005f);
        noiseda87bcd4_1563_446d_a9bf_9a85382e6752.SetSeed(1337);
        noiseda87bcd4_1563_446d_a9bf_9a85382e6752.SetNoiseType(SFastNoise.NoiseType.Perlin);
        SFastNoise noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65 = new SFastNoise();
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetFrequency(0.05f);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetSeed(1337);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetNoiseType(SFastNoise.NoiseType.Perlin);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetFractalType(SFastNoise.FractalType.FBM);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetFractalGain(0.5f);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetFractalLacunarity(2f);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetFractalOctaves(3);
        noiseaa6cdc62_3d41_43b5_a14d_3783bebf4d65.SetNoiseType(SFastNoise.NoiseType.CubicFractal);
        SFastNoise noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d = new SFastNoise();
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetFrequency(0.05f);
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetSeed(1337);
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetNoiseType(SFastNoise.NoiseType.Perlin);
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetNoiseType(SFastNoise.NoiseType.Cellular);
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetCellularDistanceFunction(SFastNoise.CellularDistanceFunction.Euclidean);
        noise8f2c3cff_4590_45bb_96ee_9c5ba9fec02d.SetCellularReturnType(SFastNoise.CellularReturnType.CellValue);
        return (noiseda87bcd4_1563_446d_a9bf_9a85382e6752.GetNoise(x + 0f, y + 0f) * 20f + noise89b3fd8b_a932_42ba_be4f_f7e1bb820182.GetNoise(x + 0f, y + 0f) * 10f);
    }
}