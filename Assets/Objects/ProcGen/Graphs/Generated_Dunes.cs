public partial struct Generated_GenerationStatics
{
	private static readonly FastNoise Noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4 = InitNoise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4();
	
	public static FastNoise InitNoise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4()
	{
		FastNoise noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4= new FastNoise();
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetFrequency(0.005f);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetSeed(1337);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetNoiseType(FastNoise.NoiseType.Perlin);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetFractalType(FastNoise.FractalType.FBM);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetFractalGain(0.5f);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetFractalLacunarity(2f);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetFractalOctaves(3);
		noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
		
		return noise72f7d1c9_7fb2_4608_9e8e_aa9df6b342c4;
	}
	
	private static readonly FastNoise Noise34a39279_8e27_4821_bcb1_4f7cc020c5ef = InitNoise34a39279_8e27_4821_bcb1_4f7cc020c5ef();
	
	public static FastNoise InitNoise34a39279_8e27_4821_bcb1_4f7cc020c5ef()
	{
		FastNoise noise34a39279_8e27_4821_bcb1_4f7cc020c5ef= new FastNoise();
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetFrequency(0.005f);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetSeed(1337);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetNoiseType(FastNoise.NoiseType.Perlin);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetFractalType(FastNoise.FractalType.FBM);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetFractalGain(-0.57f);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetFractalLacunarity(1.28f);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetFractalOctaves(3);
		noise34a39279_8e27_4821_bcb1_4f7cc020c5ef.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
		
		return noise34a39279_8e27_4821_bcb1_4f7cc020c5ef;
	}
	
	private static readonly FastNoise Noise41ca9be1_a7a4_49a0_a497_88e54af3b81d = InitNoise41ca9be1_a7a4_49a0_a497_88e54af3b81d();
	
	public static FastNoise InitNoise41ca9be1_a7a4_49a0_a497_88e54af3b81d()
	{
		FastNoise noise41ca9be1_a7a4_49a0_a497_88e54af3b81d= new FastNoise();
		noise41ca9be1_a7a4_49a0_a497_88e54af3b81d.SetFrequency(0.013f);
		noise41ca9be1_a7a4_49a0_a497_88e54af3b81d.SetSeed(1337);
		noise41ca9be1_a7a4_49a0_a497_88e54af3b81d.SetNoiseType(FastNoise.NoiseType.Perlin);
		
		return noise41ca9be1_a7a4_49a0_a497_88e54af3b81d;
	}
	
	public static float SampleDunes(float x, float y)
	{
		return Noise41ca9be1_a7a4_49a0_a497_88e54af3b81d.GetNoise(x + 0f, y + 0f) * 20f;
	}
	
}
