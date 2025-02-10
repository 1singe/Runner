public partial struct Generated_GenerationStatics
{
	private static readonly FastNoise Noiseb5af9345_7205_4752_879b_b09fd85a8846 = InitNoiseb5af9345_7205_4752_879b_b09fd85a8846();
	
	public static FastNoise InitNoiseb5af9345_7205_4752_879b_b09fd85a8846()
	{
		FastNoise noiseb5af9345_7205_4752_879b_b09fd85a8846= new FastNoise();
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetFrequency(0.005f);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetSeed(1337);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetNoiseType(FastNoise.NoiseType.Perlin);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetFractalType(FastNoise.FractalType.FBM);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetFractalGain(0.5f);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetFractalLacunarity(2f);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetFractalOctaves(3);
		noiseb5af9345_7205_4752_879b_b09fd85a8846.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
		
		return noiseb5af9345_7205_4752_879b_b09fd85a8846;
	}
	
	private static readonly FastNoise Noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7 = InitNoise26661f5e_7bd3_4098_a608_d3d63c1fe0b7();
	
	public static FastNoise InitNoise26661f5e_7bd3_4098_a608_d3d63c1fe0b7()
	{
		FastNoise noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7= new FastNoise();
		noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7.SetFrequency(0.001f);
		noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7.SetSeed(1337);
		noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7.SetNoiseType(FastNoise.NoiseType.Perlin);
		noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7.SetNoiseType(FastNoise.NoiseType.Simplex);
		
		return noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7;
	}
	
	private static readonly FastNoise Noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6 = InitNoise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6();
	
	public static FastNoise InitNoise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6()
	{
		FastNoise noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6= new FastNoise();
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetFrequency(0.005f);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetSeed(1337);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetNoiseType(FastNoise.NoiseType.Perlin);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetFractalType(FastNoise.FractalType.FBM);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetFractalGain(-0.57f);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetFractalLacunarity(1.28f);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetFractalOctaves(3);
		noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
		
		return noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6;
	}
	
	public static float SampleDunes(float x, float y)
	{
		return (Noise26661f5e_7bd3_4098_a608_d3d63c1fe0b7.GetNoise(x + 0f, y + 0f) * 100f + (Noiseb5af9345_7205_4752_879b_b09fd85a8846.GetNoise(x + 0f, y + 0f) * 10f * Noise081ea543_0cf4_4c32_8e6b_1a2e1948cfc6.GetNoise(x + 0f, y + 0f) * 10f));
	}
	
}
