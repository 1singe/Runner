using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PGG;
using UnityEngine.UI;

namespace PGG.Editor
{
    public class PGGEditorNode : UnityEditor.Experimental.GraphView.Node
    {
        public Node Node { get; private set; }
        public Port OutputPort { get; private set; }
        public List<Port> Ports { get; private set; }
        public List<Port> InputPorts { get; private set; }

        public PGGPreview Preview { get; private set; }

        public NodeInfoAttribute NodeAttribute { get; private set; }

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedNode;

        private Dictionary<FieldInfo, SerializedProperty> _propertyDictionary;

        public Dictionary<SerializedProperty, PropertyField> PropertyFields;

        public PGGView View { get; private set; }

        public void CreateOutputPort(NodeInfoAttribute attributeInfo)
        {
            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            OutputPort.portName = "";
            OutputPort.portColor = PGGPortTypes.Colors[attributeInfo.OutputType];
            Ports.Add(OutputPort);
            outputContainer.Add(OutputPort);
        }

        public void CreateInputPorts(InputAttribute inputAttributeInfo, SerializedProperty property)
        {
            // Todo multi stuff

            if (inputAttributeInfo.MultiInput)
            {
                List<SerializedProperty> properties = FetchEnumerableInnerProperties(property);
                for (int i = 0; i < properties.Count; i++)
                {
                    CreateInputPort(inputAttributeInfo, properties[i]);
                }
            }
            else
            {
                CreateInputPort(inputAttributeInfo, property);
            }
        }

        private void CreateAdditionalInputPort()
        {
            Node.AddInput();
        }

        private void CreateInputPort(InputAttribute inputAttributeInfo, SerializedProperty property)
        {
            PGGPortPropertyContainer portValueContainer = new PGGPortPropertyContainer();
            inputContainer.Add(portValueContainer);

            if (inputAttributeInfo.IsBindable)
            {
                Port Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, inputAttributeInfo.DisplayType);
                Input.portName = "";
                Input.portColor = PGGPortTypes.Colors[inputAttributeInfo.DisplayType];
                Ports.Add(Input);
                portValueContainer.Add(Input);
                InputPorts.Add(Input);
            }

            if (inputAttributeInfo.IsParameter)
            {
                PropertyField propertyField = new PropertyField(property);
                propertyField.style.flexBasis = new StyleLength(Length.Percent(100f));
                propertyField.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
                propertyField.style.alignContent = new StyleEnum<Align>(Align.Stretch);
                propertyField.style.alignItems = new StyleEnum<Align>(Align.Stretch);
                propertyField.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
                propertyField.style.flexShrink = 1f;
                propertyField.style.flexGrow = 3f;
                propertyField.label = inputAttributeInfo.DisplayName;
                propertyField.Bind(_serializedObject);
                propertyField.bindingPath = property.propertyPath;
                portValueContainer.Add(propertyField);

                propertyField.RegisterCallback<SerializedPropertyChangeEvent>(OnValueChanged, TrickleDown.TrickleDown);
            }
        }

        private void OnValueChanged(SerializedPropertyChangeEvent evt)
        {
            View.Window.OnGraphChanged();
        }

        public PGGEditorNode(Node node, SerializedObject obj, PGGView view)
        {
            AddToClassList("pgg-node");
            Node = node;
            View = view;

            Type type = node.GetType();

            NodeAttribute = type.GetCustomAttribute<NodeInfoAttribute>();

            title = NodeAttribute.NodeTitle;
            _serializedObject = obj;
            PropertyFields = new Dictionary<SerializedProperty, PropertyField>();


            Ports = new List<Port>();
            InputPorts = new List<Port>();

            string[] splits = NodeAttribute.MenuItem.Split('/');
            foreach (string split in splits)
            {
                AddToClassList(split.ToLower().Replace(' ', '-'));
            }

            name = type.Name;
            FetchSerializedProperty();

            if (!NodeAttribute.IsOutputNode)
            {
                CreateOutputPort(NodeAttribute);
            }
            else
            {
                capabilities &= ~Capabilities.Deletable;
                capabilities &= ~Capabilities.Movable;
                capabilities &= ~Capabilities.Resizable;
            }

            foreach (FieldInfo field in type.GetFields())
            {
                InputAttribute fieldAttribute = field.GetCustomAttribute<InputAttribute>();
                if (fieldAttribute != null)
                {
                    SerializedProperty property = FetchSerializedInnerProperty(field);
                    CreateInputPorts(fieldAttribute, property);
                }
            }

            // Create Preview
            Preview = new PGGPreview(_serializedObject);
            extensionContainer.Add(Preview);

            RefreshExpandedState();
        }

        public void FetchSerializedProperty()
        {
            SerializedProperty nodes = _serializedObject.FindProperty("_nodes");
            if (nodes.isArray)
            {
                int size = nodes.arraySize;
                for (int i = 0; i < size; i++)
                {
                    var node = nodes.GetArrayElementAtIndex(i);
                    var nodeID = node.FindPropertyRelative("ID");
                    if (nodeID == null)
                        continue;
                    if (nodeID.stringValue == Node.ID)
                    {
                        _serializedNode = node;
                    }
                }
            }
        }

        public void DrawPreview()
        {
            Preview.WriteToImage(Node.GraphAssetReference.ProcessGraph(Node));
        }

        public SerializedProperty FetchSerializedInnerProperty(FieldInfo field)
        {
            if (_serializedNode == null)
            {
                FetchSerializedProperty();
            }

            return _serializedNode.FindPropertyRelative(field.Name);
        }

        public List<SerializedProperty> FetchEnumerableInnerProperties(SerializedProperty property)
        {
            List<SerializedProperty> innerProperties = new List<SerializedProperty>();
            if (property != null && property.isArray)
            {
                int insideSize = property.arraySize;
                for (int j = 0; j < insideSize; j++)
                {
                    innerProperties.Add(property.GetArrayElementAtIndex(j));
                }
            }

            return innerProperties;
        }

        public sealed override string title
        {
            get { return base.title; }
            set { base.title = value; }
        }

        public void SavePosition()
        {
            Node.SetPosition(GetPosition());
        }
    }
}