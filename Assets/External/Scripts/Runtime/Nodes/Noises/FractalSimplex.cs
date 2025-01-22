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

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.SimplexFractal);");
        }
    }
}