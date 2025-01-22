using System.Collections.Generic;

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

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.PerlinFractal);");
        }
    }
}