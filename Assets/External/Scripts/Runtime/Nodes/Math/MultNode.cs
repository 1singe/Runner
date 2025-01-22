using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Multiply", "Math/Multiply")]
    public class MultNode : Node
    {
        [SerializeField] [Input(false, typeof(float), true, false)]
        public float A = 1f;

        [SerializeField] [Input(false, typeof(float), true, false)]
        public float B = 1f;

        public override float ProcessSelf(float x, float y)
        {
            float value = 1f;
            if (InputIDs.Count >= 2)
            {
                value = ProcessNode(0, x, y) * ProcessNode(1, x, y);
            }

            return value;
        }

        public override string BakeProcess(string Input)
        {
            return "(" + BakeProcessNext(0, Input) + " * " + BakeProcessNext(1, Input) + ")";
        }
    }
}