using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PGG
{
    public class RawFormat : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Node
    {
        [SerializeField] public string ID;
        protected string _id;
        protected IFormatProvider _format;
        [SerializeField] public List<string> InputIDs;
        [SerializeField] public Rect Position;
        [SerializeField] public GraphAsset GraphAssetReference;

        public Node()
        {
            NewGuid();
            _id = ID.Replace('-', '_');
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

        public virtual float ProcessNode(int index, float x, float y)
        {
            if (GetNextNode(index) != null)
                return GetNextNode(index).ProcessSelf(x, y);
            return 0f;
        }

        public virtual float ProcessSelf(float x, float y)
        {
            return 0f;
        }

        public virtual void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
        }

        public virtual string BakeProcess(string Input)
        {
            return "";
        }

        public Node GetNextNode(int index)
        {
            if (InputIDs != null && index < InputIDs.Count)
            {
                string id = InputIDs[index];
                if (GraphAssetReference != null && GraphAssetReference.NodeDictionary != null && GraphAssetReference.NodeDictionary.ContainsKey(id))
                {
                    return GraphAssetReference.NodeDictionary[InputIDs[index]];
                }

                return null;
            }

            return null;
        }

        public virtual string BakeProcessNext(int index, string input)
        {
            return GetNextNode(index).BakeProcess(input);
        }
    }
}