using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace PGG.Editor
{
    public class PGGView : GraphView
    {
        private GraphAsset _graphAsset;
        private SerializedObject _serializedObject;
        public PGGWindow Window { get; private set; }

        public List<PGGEditorNode> EditorNodes;
        public Dictionary<string, PGGEditorNode> NodeDictionary;
        public Dictionary<Edge, Connection> ConnectionDictionary;

        private PGGSearchWindowProvider _searchProvider;

        public PGGView(SerializedObject obj, PGGWindow window)
        {
            _serializedObject = obj;
            _graphAsset = (GraphAsset)_serializedObject.targetObject;
            EditorNodes = new List<PGGEditorNode>();
            NodeDictionary = new Dictionary<string, PGGEditorNode>();
            ConnectionDictionary = new Dictionary<Edge, Connection>();
            _searchProvider = ScriptableObject.CreateInstance<PGGSearchWindowProvider>();
            _searchProvider.View = this;
            nodeCreationRequest = ShowSearchWindow;
            Window = window;

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/External/Resource/Editor/StyleSheets/ProceduralGenerationEditorStyleSheet.uss");
            styleSheets.Add(styleSheet);

            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new ContentZoomer());

            DrawNodes();
            DrawConnections();

            BindAndUpdate();

            graphViewChanged += OnGraphViewChangedEvent;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();

            foreach (var node in EditorNodes)
            {
                allPorts.AddRange(node.Ports);
            }

            foreach (var port in allPorts)
            {
                if (port == startPort)
                {
                    continue;
                }

                if (port.node == startPort.node)
                {
                    continue;
                }

                if (port.direction == startPort.direction)
                {
                    continue;
                }

                if (port.portType == startPort.portType)
                {
                    ports.Add(port);
                }
            }

            return ports;
        }

        private GraphViewChange OnGraphViewChangedEvent(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                Undo.RecordObject(_serializedObject.targetObject, "Move Node");
                foreach (var editorNode in graphViewChange.movedElements.OfType<PGGEditorNode>())
                {
                    editorNode.SavePosition();
                }
            }


            if (graphViewChange.elementsToRemove != null)
            {
                List<PGGEditorNode> editorNodes = graphViewChange.elementsToRemove.OfType<PGGEditorNode>().ToList();
                if (editorNodes.Count > 0)
                {
                    Undo.RecordObject(_serializedObject.targetObject, "Remove Node");

                    for (int i = editorNodes.Count - 1; i >= 0; i--)
                    {
                        if (editorNodes[i].Node is OutputNode)
                        {
                            continue;
                        }

                        RemoveNode(editorNodes[i]);
                    }
                }

                foreach (Edge edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    RemoveConnection(edge);
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(_serializedObject.targetObject, "Link Node");
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    CreateEdge(edge);
                }
            }

            return graphViewChange;
        }

        private void CreateEdge(Edge edge)
        {
            PGGEditorNode inputNode = (PGGEditorNode)edge.input.node;
            int inputIndex = inputNode.Ports.IndexOf(edge.input);
            PGGEditorNode outputNode = (PGGEditorNode)edge.output.node;
            int outputIndex = outputNode.Ports.IndexOf(edge.output);

            Connection connection = new Connection(inputNode.Node.ID, inputIndex, outputNode.Node.ID, outputIndex);
            _graphAsset.Connections.Add(connection);
            ConnectionDictionary.Add(edge, connection);
        }

        public void RemoveNode(PGGEditorNode editorNode)
        {
            _graphAsset.Nodes.Remove(editorNode.Node);
            NodeDictionary.Remove(editorNode.Node.ID);
            EditorNodes.Remove(editorNode);

            _serializedObject.Update();
        }

        private void DrawNodes()
        {
            foreach (Node node in _graphAsset.Nodes)
            {
                AddNodeToGraph(node);
            }
        }


        private void DrawConnections()
        {
            if (_graphAsset.Connections == null)
            {
                return;
            }

            foreach (Connection connection in _graphAsset.Connections)
            {
                DrawConnection(connection);
            }
        }

        private void RemoveConnection(Edge edge)
        {
            if (ConnectionDictionary.TryGetValue(edge, out Connection connection))
            {
                _graphAsset.Connections.Remove(connection);
                ConnectionDictionary.Remove(edge);
            }
        }

        private void DrawConnection(Connection connection)
        {
            PGGEditorNode inputNode = GetNode(connection.In.NodeId);
            PGGEditorNode outputNode = GetNode(connection.Out.NodeId);
            if (inputNode == null) return;
            if (outputNode == null) return;

            Port inputPort = inputNode.Ports[connection.In.PortIndex];
            Port outputPort = outputNode.Ports[connection.Out.PortIndex];

            Edge edge = inputPort.ConnectTo(outputPort);
            AddElement(edge);
            ConnectionDictionary.Add(edge, connection);
        }

        private PGGEditorNode GetNode(string inNodeId)
        {
            PGGEditorNode node = null;
            NodeDictionary.TryGetValue(inNodeId, out node);
            return node;
        }

        private void ShowSearchWindow(NodeCreationContext obj)
        {
            _searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), _searchProvider);
        }


        public void Add(Node node)
        {
            Undo.RecordObject(_serializedObject.targetObject, "Create Node");
            _graphAsset.Nodes.Add(node);
            _serializedObject.Update();
            AddNodeToGraph(node);

            BindAndUpdate();
        }

        private void BindAndUpdate()
        {
            _serializedObject.Update();
            this.Bind(_serializedObject);
        }


        private void AddNodeToGraph(Node node)
        {
            PGGEditorNode editorNode = new PGGEditorNode(node, _serializedObject, this);
            editorNode.SetPosition(node.Position);
            EditorNodes.Add(editorNode);
            NodeDictionary.Add(node.ID, editorNode);

            AddElement(editorNode);
        }

        public PGGEditorNode GetOutputEditorNode()
        {
            return EditorNodes.Find((node => node.Node.GetType() == typeof(OutputNode)));
        }
    }
}