using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Subtract", "Math/Subtract")]
    public class SubtractNode : Node
    {
        [SerializeField] [Input(true, typeof(float))]
        public List<float> Inputs = new List<float>(2) { 0f, 0f };

        public override float ProcessSelf(float x, float y)
        {
            float sum = 0f;
            if (InputIDs == null)
            {
                return sum;
            }

            foreach (string ID in InputIDs)
            {
                sum -= ProcessNode(ID, x, y);
            }

            return sum;
        }

        public override void AddInput()
        {
            Inputs.Add(0f);
        }
    }
}