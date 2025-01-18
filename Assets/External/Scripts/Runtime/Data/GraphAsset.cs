using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PGG
{
    [CreateAssetMenu(fileName = "New Graph", menuName = "ScriptableObjects/Procedural/Graph")]
    public class GraphAsset : ScriptableObject
    {
        public const int TEXREZ = 64;
        [SerializeReference] private List<Node> _nodes;

        public Node OutputNode;

        [SerializeField] public List<Connection> Connections;
        [SerializeField] public Dictionary<string, Node> NodeDictionary;

        public List<Node> Nodes => _nodes;

        public GraphAsset()
        {
            _nodes = new List<Node>();
            Connections = new List<Connection>();
            OutputNode = new OutputNode();
            _nodes.Add(OutputNode);
        }

        public void Cook()
        {
            NodeDictionary = new Dictionary<string, Node>();
            OutputNode = GetOutputNode();

            foreach (Node node in Nodes)
            {
                node.InputIDs = new List<string>();
                NodeDictionary.Add(node.ID, node);
                node.GraphAssetReference = this;
                node.Init();
            }

            foreach (var Connection in Connections)
            {
                var inputPort = Connection.In;
                var outputPort = Connection.Out;
                GetNode(inputPort.NodeId).InputIDs.Add(outputPort.NodeId);
            }
        }

        public MinMaxHeightMap ProcessGraph(Node node)
        {
            MinMaxHeightMap heightMap = new MinMaxHeightMap(TEXREZ);
            for (int y = 0; y < TEXREZ; y++)
            {
                for (int x = 0; x < TEXREZ; x++)
                {
                    float value = node.ProcessSelf(x, y);
                    if (value > heightMap.Max)
                        heightMap.Max = value;
                    if (value < heightMap.Min)
                        heightMap.Min = value;
                    heightMap.HeightMap[x, y] = value;
                }
            }

            return heightMap;
        }

        public float SampleGraphAtPos(float x, float y)
        {
            return OutputNode.ProcessSelf(x, y);
        }

        public float ProcessNode(Node node, float x, float y)
        {
            return node.ProcessSelf(x, y);
        }

        public Node GetNode(string id)
        {
            return NodeDictionary[id];
        }

        public Node GetOutputNode()
        {
            return Nodes.OfType<OutputNode>().First();
        }
    }
}