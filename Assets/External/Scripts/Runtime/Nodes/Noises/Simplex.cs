namespace PGG
{
    [NodeInfo("Simplex Noise", "Sample/Simplex", false, typeof(PGGPortTypes.NoisePort))]
    public class Simplex : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Simplex);
        }
    }
}