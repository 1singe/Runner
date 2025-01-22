using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Subtract", "Math/Subtract")]
    public class SubtractNode : Node
    {
        [SerializeField] [Input(false, typeof(float), true, false)]
        public float A = 0f;

        [SerializeField] [Input(false, typeof(float), true, false)]
        public float B = 0f;

        public override float ProcessSelf(float x, float y)
        {
            float sum = 0f;
            if (InputIDs.Count >= 2)
            {
                sum = ProcessNode(0, x, y) - ProcessNode(1, x, y);
            }

            return sum;
        }

        public override string BakeProcess(string Input)
        {
            return "(" + BakeProcessNext(0, Input) + " - " + BakeProcessNext(1, Input) + ")";
        }
    }
}