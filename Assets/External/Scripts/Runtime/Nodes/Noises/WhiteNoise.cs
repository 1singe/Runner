using System.Collections.Generic;

namespace PGG
{
    [NodeInfo("White Noise", "Sample/White Noise", false, typeof(PGGPortTypes.NoisePort))]
    public class WhiteNoise : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
        }

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.WhiteNoise);");
        }
    }
}