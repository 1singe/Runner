using UnityEngine;

namespace PGG
{
    [NodeInfo("Offset", "Sample/Offset")]
    public class OffsetNode : Node
    {
        [Input(false, typeof(float))] public float Input = 0f;

        [Input(false, typeof(Vector2), true, true)]
        public Vector2 Offset;
    }
}