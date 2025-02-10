using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
        private Dictionary<string, List<string>> InitLines;

        [HideInInspector] public UnityEvent OnGraphCooked;

        public GraphAsset()
        {
            _nodes = new List<Node>();
            Connections = new List<Connection>();
            OutputNode = new OutputNode();
            _nodes.Add(OutputNode);
            InitLines = new Dictionary<string, List<string>>();
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
            InitLines = new Dictionary<string, List<string>>();

            File.WriteAllText(filePath, "public partial struct Generated_GenerationStatics" + ScopeIn() + GenerateContent() + ScopeOut());


            _indent = 0;

            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();

            return filePath != "";
        }

        private string GenerateContent()
        {
            foreach (var node in Nodes)
            {
                node.BakeInit(ref InitLines);
            }

            return GenerateInit() + "public static float Sample" + name + "(float x, float y)" + ScopeIn() + OutputNode.BakeProcess("") + ScopeOut();
        }

        private string GenerateInit()
        {
            string init = "";
            foreach (var entry in InitLines)
            {
                init += "private static readonly FastNoise Noise" + entry.Key + " = InitNoise" + entry.Key + "();" + JumpLine() + JumpLine();
                init += "public static FastNoise InitNoise" + entry.Key + "()" + ScopeIn();
                foreach (var line in entry.Value)
                {
                    init += line + JumpLine();
                }

                init += JumpLine();
                init += "return noise" + entry.Key + ";" + ScopeOut() + JumpLine();
            }

            return init;
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

        public MinMaxHeightMap ProcessGraph(Node node, int Size = TEXREZ, float offsetX = 0f, float offsetY = 0f)
        {
            MinMaxHeightMap heightMap = new MinMaxHeightMap(Size);
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    float value = node.ProcessSelf(x + offsetX, y + offsetY);
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