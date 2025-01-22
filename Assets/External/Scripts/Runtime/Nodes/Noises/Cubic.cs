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

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.Cubic);");
        }
    }
}