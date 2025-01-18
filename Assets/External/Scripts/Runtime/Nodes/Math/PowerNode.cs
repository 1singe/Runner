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
            if (InputIDs == null || InputIDs.Count < 1)
            {
                return 1f;
            }

            return Mathf.Pow(ProcessNode(InputIDs[0], x, y), Pow);
        }
    }
}