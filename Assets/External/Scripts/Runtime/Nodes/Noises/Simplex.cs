using System.Collections.Generic;

namespace PGG
{
    [NodeInfo("Simplex Noise", "Sample/Simplex", false, typeof(PGGPortTypes.NoisePort))]
    public class Simplex : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Simplex);
        }

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.Simplex);");
        }
    }
}