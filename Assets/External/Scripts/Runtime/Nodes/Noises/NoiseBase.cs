using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace PGG
{
    public abstract class NoiseBase : Node
    {
        [SerializeReference] protected FastNoise _noise = new FastNoise(1337);

        [SerializeField] [Input(false, typeof(float), false, true, "Sample Frequency")]
        public float Frequency = 0.05f;

        [SerializeField] [Input(false, typeof(float), false, true, "Amplitude")]
        public float Amplitude = 10f;

        [SerializeField] [Input(false, typeof(Vector2), false, true, "Offset")]
        public Vector2 Offset = Vector2.zero;

        [SerializeField] [Input(false, typeof(int), false, true, "Seed")]
        public int Seed = 1337;

        public override void Init()
        {
            _noise = new FastNoise();
            _noise.SetFrequency(Frequency);
            _noise.SetSeed(Seed);
            _noise.SetNoiseType(FastNoise.NoiseType.Perlin);
        }

        public override float ProcessSelf(float x, float y)
        {
            return _noise.GetNoise(x + Offset.x, y + Offset.y) * Amplitude;
        }

        public override void BakeInit(ref List<string> InitLines)
        {
            InitLines.Add("SFastNoise noise" + _id + "= new SFastNoise();");
            InitLines.Add("noise" + _id + ".SetFrequency(" + Frequency.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f);");
            InitLines.Add("noise" + _id + ".SetSeed(" + Seed + ");");
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.Perlin);");
        }

        public override string BakeProcess(string Input)
        {
            return "noise" + _id + ".GetNoise(x + " + Offset.x.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f, y + " + Offset.y.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f) * " + Amplitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f";
        }
    }
}