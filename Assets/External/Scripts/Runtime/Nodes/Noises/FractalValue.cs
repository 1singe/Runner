namespace PGG
{
    [NodeInfo("Fractal Value Noise", "Sample/Fractal Value", false, typeof(PGGPortTypes.NoisePort))]
    public class FractalValue : FractalBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.ValueFractal);
        }
    }
}