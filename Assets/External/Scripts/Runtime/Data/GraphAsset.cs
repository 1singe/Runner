using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using UnityEditor;
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

        private int _indent = 0;
        private List<string> InitLines;

        public GraphAsset()
        {
            _nodes = new List<Node>();
            Connections = new List<Connection>();
            OutputNode = new OutputNode();
            _nodes.Add(OutputNode);
            InitLines = new List<string>();
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

        public bool Bake()
        {
            string filePath = OpenGeneratedFile();
            InitLines = new List<string>();

            File.WriteAllText(filePath, "public partial struct Generated_GenerationStatics" + ScopeIn() + GenerateContent() + ScopeOut());


            _indent = 0;

            return filePath != "";
        }

        private string GenerateContent()
        {
            foreach (var node in Nodes)
            {
                node.BakeInit(ref InitLines);
            }

            return "public static float Sample" + name + "(float x, float y)" + ScopeIn() + GenerateInit() + OutputNode.BakeProcess("") + ScopeOut();
        }

        private string GenerateInit()
        {
            string ret = "";
            foreach (var line in InitLines)
            {
                ret += line + JumpLine();
            }

            return ret;
        }


        private string ScopeIn()
        {
            var ret = JumpLine() + "{";
            _indent++;
            ret += JumpLine();
            return ret;
        }

        private string JumpLine()
        {
            return "\n" + Indent();
        }

        private string ScopeOut()
        {
            _indent--;
            var ret = JumpLine() + "}";
            ret += JumpLine();
            return ret;
        }

        private string Indent()
        {
            string indent = "";
            for (int i = 0; i < _indent; i++)
                indent += "\t";
            return indent;
        }

        private string OpenGeneratedFile()
        {
            string AssetPath = AssetDatabase.GetAssetPath(this);
            string outerFolderPath = "";
            var pathArray = AssetPath.Split('/');
            for (int i = 1; i < pathArray.Length - 1; i++)
            {
                var subString = pathArray[i];
                outerFolderPath += "/" + subString;
            }

            outerFolderPath += '/';

            return Application.dataPath + outerFolderPath + "Generated_" + name + ".cs";
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