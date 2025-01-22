using System.Collections.Generic;

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

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.CubicFractal);");
        }
    }
}