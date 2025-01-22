using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("One Minus", "Math/One Minus")]
    public class OneMinus : Node
    {
        [SerializeField] [Input(false, typeof(float), true, false)]
        public float Input = 0f;

        public override float ProcessSelf(float x, float y)
        {
            return 1f - ProcessNode(0, x, y);
        }

        public override string BakeProcess(string Input)
        {
            return "( 1f -" + BakeProcessNext(0, Input) + ")";
        }
    }
}