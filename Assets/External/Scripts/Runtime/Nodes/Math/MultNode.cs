using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Multiply", "Math/Multiply")]
    public class MultNode : Node
    {
        [SerializeField] [Input(true, typeof(float))]
        public List<float> Inputs = new List<float>(2) { 1f, 1f };

        public override float ProcessSelf(float x, float y)
        {
            float value = 1f;
            for (int i = 0; i < InputIDs.Count; i++)
            {
                value *= ProcessNode(i, x, y);
            }

            return value;
        }


        public override void AddInput()
        {
            Inputs.Add(1f);
        }

        public override string BakeProcess(string Input)
        {
            return "(" + BakeProcessNext(0, Input) + " * " + BakeProcessNext(1, Input) + ")";
        }
    }
}