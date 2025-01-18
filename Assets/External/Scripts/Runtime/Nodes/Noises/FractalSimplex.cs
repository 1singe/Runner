namespace PGG
{
    [NodeInfo("Fractal Simplex Noise", "Sample/Fractal Simplex", false, typeof(PGGPortTypes.NoisePort))]
    public class FractalSimplex : FractalBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
        }
    }
}