using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PGG
{
    [Serializable]
    public class Node
    {
        [SerializeField] public string ID;
        [SerializeField] public List<string> InputIDs;
        [SerializeField] public Rect Position;
        [SerializeField] public GraphAsset GraphAssetReference;

        public Node()
        {
            NewGuid();
        }

        public virtual void AddInput()
        {
        }

        public virtual void Init()
        {
        }

        private void NewGuid()
        {
            ID = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            Position = position;
        }

        public virtual float ProcessNode(string id, float x, float y)
        {
            return GraphAssetReference.ProcessNode(GraphAssetReference.NodeDictionary[id], x, y);
        }

        public virtual float ProcessSelf(float x, float y)
        {
            return 0f;
        }
    }
}