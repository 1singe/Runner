using System.Collections.Generic;

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

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.ValueFractal);");
        }
    }
}