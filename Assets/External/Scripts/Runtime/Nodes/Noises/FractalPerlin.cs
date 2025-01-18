namespace PGG
{
    [NodeInfo("Fractal Perlin Noise", "Sample/Fractal Perlin", false, typeof(PGGPortTypes.NoisePort))]
    public class FractalPerlin : FractalBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
        }
    }
}