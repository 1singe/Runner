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
            if (InputIDs == null || InputIDs.Count < 2)
            {
                return 0f;
            }

            return ProcessNode(InputIDs[0], x, y) / ProcessNode(InputIDs[1], x, y);
        }
    }
}