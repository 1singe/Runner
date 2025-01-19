using System.Collections.Generic;

namespace PGG
{
    [NodeInfo("Cubic Noise", "Sample/Cubic", false, typeof(PGGPortTypes.NoisePort))]
    public class Cubic : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Cubic);
        }

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.Cubic);");
        }
    }
}