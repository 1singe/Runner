namespace PGG
{
    [NodeInfo("Fractal Cubic Noise", "Sample/Fractal Cubic", false, typeof(PGGPortTypes.NoisePort))]
    public class FractalCubic : FractalBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.CubicFractal);
        }
    }
}