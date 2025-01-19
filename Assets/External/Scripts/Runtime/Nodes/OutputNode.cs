using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Output", "Core/Output", true)]
    public class OutputNode : Node
    {
        [SerializeField] [Input(false, typeof(float), true, false)]
        public float Input;

        public override float ProcessSelf(float x, float y)
        {
            float value = ProcessNode(0, x, y);
            return value;
        }

        public override string BakeProcess(string Input)
        {
            return Input + "return " + GetNextNode(0).BakeProcess(Input) + ";";
        }
    }
}