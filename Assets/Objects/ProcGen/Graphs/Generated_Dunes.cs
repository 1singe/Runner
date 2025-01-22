public partial struct Generated_GenerationStatics
{
	private static readonly FastNoise Noise632c1c1d_5fb1_40ac_af81_8326341d16b3 = InitNoise632c1c1d_5fb1_40ac_af81_8326341d16b3();
	
	public static FastNoise InitNoise632c1c1d_5fb1_40ac_af81_8326341d16b3()
	{
		FastNoise noise632c1c1d_5fb1_40ac_af81_8326341d16b3= new FastNoise();
		noise632c1c1d_5fb1_40ac_af81_8326341d16b3.SetFrequency(0.003f);
		noise632c1c1d_5fb1_40ac_af81_8326341d16b3.SetSeed(1337);
		noise632c1c1d_5fb1_40ac_af81_8326341d16b3.SetNoiseType(FastNoise.NoiseType.Perlin);
		
		return noise632c1c1d_5fb1_40ac_af81_8326341d16b3;
	}
	
	private static readonly FastNoise Noise85c7536d_61a0_4bf3_af05_94db33021083 = InitNoise85c7536d_61a0_4bf3_af05_94db33021083();
	
	public static FastNoise InitNoise85c7536d_61a0_4bf3_af05_94db33021083()
	{
		FastNoise noise85c7536d_61a0_4bf3_af05_94db33021083= new FastNoise();
		noise85c7536d_61a0_4bf3_af05_94db33021083.SetFrequency(0.01f);
		noise85c7536d_61a0_4bf3_af05_94db33021083.SetSeed(1337);
		noise85c7536d_61a0_4bf3_af05_94db33021083.SetNoiseType(FastNoise.NoiseType.Perlin);
		
		return noise85c7536d_61a0_4bf3_af05_94db33021083;
	}
	
	private static readonly FastNoise Noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd = InitNoiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd();
	
	public static FastNoise InitNoiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd()
	{
		FastNoise noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd= new FastNoise();
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetFrequency(0.002f);
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetSeed(1337);
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetNoiseType(FastNoise.NoiseType.Perlin);
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetNoiseType(FastNoise.NoiseType.Cellular);
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
		noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
		
		return noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd;
	}
	
	public static float SampleDunes(float x, float y)
	{
		return ((Noise632c1c1d_5fb1_40ac_af81_8326341d16b3.GetNoise(x + 0f, y + 0f) * 200f + Noise85c7536d_61a0_4bf3_af05_94db33021083.GetNoise(x + 0f, y + 0f) * 30f) * (10f + ( 1f -Noiseb37cc8c1_5f3a_46c7_bbd6_bfc91dfab6bd.GetCellular(x + 0f, y + 0f) * 1f)));
	}
	
}
