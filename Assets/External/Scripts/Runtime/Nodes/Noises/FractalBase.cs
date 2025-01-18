using UnityEngine;

namespace PGG
{
    public abstract class FractalBase : NoiseBase
    {
        [SerializeField] [Input(false, typeof(FastNoise.FractalType), false, true, "Fractal Type")]
        public FastNoise.FractalType FractalType = FastNoise.FractalType.FBM;

        [SerializeField] [Input(false, typeof(int), false, true, "Fractal Octaves")]
        public int Octaves = 3;

        [SerializeField] [Input(false, typeof(float), false, true, "Fractal Gain")]
        public float Gain = 0.5f;

        [SerializeField] [Input(false, typeof(float), false, true, "Fractal Lacunarity")]
        public float Lacunarity = 2f;

        public override void Init()
        {
            base.Init();
            _noise.SetFractalType(FractalType);
            _noise.SetFractalGain(Gain);
            _noise.SetFractalLacunarity(Lacunarity);
            _noise.SetFractalOctaves(Octaves);
        }
    }
}