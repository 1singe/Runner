using System.Collections.Generic;

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

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.SimplexFractal);");
        }
    }
}