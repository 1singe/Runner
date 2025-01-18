namespace PGG
{
    [NodeInfo("Cubic Noise", "Sample/Cubic", false, typeof(PGGPortTypes.NoisePort))]
    public class Cubic : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Cubic);
        }
    }
}