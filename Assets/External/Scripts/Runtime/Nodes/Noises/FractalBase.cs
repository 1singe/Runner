using System.Collections.Generic;
using System.Globalization;
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

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetFractalType(SFastNoise.FractalType." + FractalType + ");");
            InitLines.Add("noise" + _id + ".SetFractalGain(" + Gain.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f);");
            InitLines.Add("noise" + _id + ".SetFractalLacunarity(" + Lacunarity.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f);");
            InitLines.Add("noise" + _id + ".SetFractalOctaves(" + Octaves + ");");
        }
    }
}