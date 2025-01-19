using UnityEngine;

namespace PGG
{
    [NodeInfo("Divide", "Math/Divide")]
    public class DivideNode : Node
    {
        [SerializeField] [Input(false, typeof(float))]
        public float A = 0f;

        [SerializeField] [Input(false, typeof(float))]
        public float B = 1f;

        public override float ProcessSelf(float x, float y)
        {
            return ProcessNode(0, x, y) / ProcessNode(1, x, y);
        }

        public override string BakeProcess(string Input)
        {
            return "(" + BakeProcessNext(0, Input) + " / " + BakeProcessNext(1, Input) + ")";
        }
    }
}