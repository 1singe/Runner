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
            float value = 0f;
            if (InputIDs != null && InputIDs.Count > 0)
            {
                value = ProcessNode(InputIDs[0], x, y);
            }

            return value;
        }
    }
}