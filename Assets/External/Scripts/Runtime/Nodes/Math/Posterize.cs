using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Power", "Math/Power")]
    public class Posterize : Node
    {
        [SerializeField] [Input(true, typeof(float), true, false)]
        public float Input = 0f;

        [SerializeField] [Input(true, typeof(float), false)]
        public List<float> Steps = new List<float>(2) { 0.3f, 0.6f };

        public override float ProcessSelf(float x, float y)
        {
            if (InputIDs == null || InputIDs.Count < 1)
            {
                return 0f;
            }

            for (int i = 0; i < Steps.Count; i++)
            {
            }

            return Mathf.Min(ProcessNode(0, x, y), Steps[0]);
        }
    }
}