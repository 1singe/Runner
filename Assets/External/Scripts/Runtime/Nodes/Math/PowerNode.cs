using System.Globalization;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Power", "Math/Power")]
    public class PowerNode : Node
    {
        [SerializeField] [Input(false, typeof(float))]
        public float Input = 0f;

        [SerializeField] [Input(false, typeof(float), false, true)]
        public float Pow = 1f;

        public override float ProcessSelf(float x, float y)
        {
            return Mathf.Pow(ProcessNode(0, x, y), Pow);
        }

        public override string BakeProcess(string Input)
        {
            return "(Mathf.Pow(" + BakeProcessNext(0, Input) + ", " + Pow.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f));";
        }
    }
}