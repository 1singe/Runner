namespace PGG
{
    [NodeInfo("White Noise", "Sample/White Noise", false, typeof(PGGPortTypes.NoisePort))]
    public class WhiteNoise : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
        }
    }
}