using UnityEngine;

namespace PGG
{
    [NodeInfo("Perlin Noise", "Sample/Perlin", false, typeof(PGGPortTypes.NoisePort))]
    public class Perlin : NoiseBase
    {
        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Perlin);
        }
    }
}