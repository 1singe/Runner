using System.Collections.Generic;

namespace PGG
{
    [NodeInfo("Value Noise", "Sample/Value", false, typeof(PGGPortTypes.NoisePort))]
    public class ValueNoise : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Value);
        }

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.Value);");
        }
    }
}