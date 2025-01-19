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

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.Simplex);");
        }
    }
}