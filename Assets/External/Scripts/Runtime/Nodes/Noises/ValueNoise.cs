namespace PGG
{
    [NodeInfo("Value Noise", "Sample/Value", false, typeof(PGGPortTypes.NoisePort))]
    public class ValueNoise : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Value);
        }
    }
}