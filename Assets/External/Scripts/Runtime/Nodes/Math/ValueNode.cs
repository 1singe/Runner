using System.Globalization;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Float Value", "Value/Float")]
    public class ValueNode : Node
    {
        [SerializeField] [Input(false, typeof(float), false, true)]
        public float Value = 0f;

        public override float ProcessSelf(float x, float y)
        {
            return Value;
        }

        public override string BakeProcess(string Input)
        {
            return Value.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f";
        }
    }
}