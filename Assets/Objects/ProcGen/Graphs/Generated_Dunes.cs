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
		return (noisea2bf61ff_5707_47ac_9b1c_613f4243e145.GetNoise(x + 0f, y + 0f) * 20f + noise66302210_391a_47e8_b515_6739e0c7cd6f.GetNoise(x + 0f, y + 0f) * 10f);
	}
	
}
