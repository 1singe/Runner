public partial struct Generated_GenerationStatics
{
	public static float SampleDunes(float x, float y)
	{
		SFastNoise noise66302210_391a_47e8_b515_6739e0c7cd6f= new SFastNoise();
		noise66302210_391a_47e8_b515_6739e0c7cd6f.SetFrequency(0.015f);
		noise66302210_391a_47e8_b515_6739e0c7cd6f.SetSeed(1337);
		noise66302210_391a_47e8_b515_6739e0c7cd6f.SetNoiseType(SFastNoise.NoiseType.Perlin);
		SFastNoise noisea2bf61ff_5707_47ac_9b1c_613f4243e145= new SFastNoise();
		noisea2bf61ff_5707_47ac_9b1c_613f4243e145.SetFrequency(0.005f);
		noisea2bf61ff_5707_47ac_9b1c_613f4243e145.SetSeed(1337);
		noisea2bf61ff_5707_47ac_9b1c_613f4243e145.SetNoiseType(SFastNoise.NoiseType.Perlin);
		SFastNoise noised1429345_e38e_4cae_806f_bd829d01f93d= new SFastNoise();
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetFrequency(0.05f);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetSeed(1337);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetNoiseType(SFastNoise.NoiseType.Perlin);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetFractalType(SFastNoise.FractalType.FBM);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetFractalGain(0.5f);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetFractalLacunarity(2f);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetFractalOctaves(3);
		noised1429345_e38e_4cae_806f_bd829d01f93d.SetNoiseType(SFastNoise.NoiseType.CubicFractal);
		SFastNoise noisea1ee460e_5228_4b83_ba49_2c37ce9a2996= new SFastNoise();
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetFrequency(0.05f);
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetSeed(1337);
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetNoiseType(SFastNoise.NoiseType.Perlin);
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetNoiseType(SFastNoise.NoiseType.Cellular);
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetCellularDistanceFunction(SFastNoise.CellularDistanceFunction.Euclidean);
		noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.SetCellularReturnType(SFastNoise.CellularReturnType.CellValue);
		return ((noisea2bf61ff_5707_47ac_9b1c_613f4243e145.GetNoise(x + 0f, y + 0f) * 20f + noise66302210_391a_47e8_b515_6739e0c7cd6f.GetNoise(x + 0f, y + 0f) * 10f) * noisea1ee460e_5228_4b83_ba49_2c37ce9a2996.GetNoise(x + 0f, y + 0f) * 10f);
	}
	
}
