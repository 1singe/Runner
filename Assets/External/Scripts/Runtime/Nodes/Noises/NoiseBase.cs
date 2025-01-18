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
    }
}