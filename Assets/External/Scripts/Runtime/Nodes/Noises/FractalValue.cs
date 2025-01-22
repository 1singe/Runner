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

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.ValueFractal);");
        }
    }
}