using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

namespace PGG.Editor
{
    public class PGGWindow : EditorWindow
    {
        public GraphAsset _currentGraph { get; private set; }
        [SerializeField] private SerializedObject _serializedObject;
        [SerializeField] private PGGView _currentView;
        [SerializeField] private PGGMainPreview _mainPreview;
        private PGGEditorNode _selectedNode;


        public static void Open(GraphAsset asset)
        {
            PGGWindow[] windows = Resources.FindObjectsOfTypeAll<PGGWindow>();
            foreach (var window in windows)
            {
                if (window._currentGraph == asset)
                {
                    window.Focus();
                    return;
                }
            }

            PGGWindow newWindow = CreateWindow<PGGWindow>(typeof(PGGWindow), typeof(SceneView));
            newWindow.titleContent = new GUIContent($"{asset.name}", EditorGUIUtility.ObjectContent(null, typeof(GraphAsset)).image);
            newWindow.Load(asset);
        }

        private void OnEnable()
        {
            if (_currentGraph != null)
            {
                DrawGraph();
            }
        }

        private void OnGUI()
        {
            bool previousSaveState = hasUnsavedChanges;
            _selectedNode = null;
            if (_currentGraph != null)
            {
                if (EditorUtility.IsDirty(_currentGraph))
                {
                    hasUnsavedChanges = true;
                }
                else
                {
                    hasUnsavedChanges = false;
                }
            }

            if (previousSaveState && !hasUnsavedChanges)
            {
                OnSave();
            }
        }

        private void OnSave()
        {
            SaveChanges();
            _currentGraph.Bake();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }


        private void Load(GraphAsset asset)
        {
            _currentGraph = asset;
            bool isValid = ValidateOrFixGraph();
            Assert.IsTrue(isValid);

            DrawGraph();

            _currentView.AddToSelection(_currentView.GetOutputEditorNode());
            _currentView.UpdateViewTransform(_currentView.GetOutputEditorNode().contentRect.center, new Vector3(0.8f, 0.8f, 0.8f));
        }

        public override void SaveChanges()
        {
            if (ValidateOrFixGraph())
            {
                base.SaveChanges();
                _currentGraph.Cook();

                _currentView.EditorNodes.ForEach((e) => e.DrawPreview());
            }
        }

        private bool ValidateOrFixGraph()
        {
            IEnumerable<OutputNode> outputNodes = _currentGraph.Nodes.OfType<OutputNode>();
            List<OutputNode> enumerable = outputNodes.ToList();

            if (enumerable.Count() == 1)
            {
                return true;
            }
            else if (!enumerable.Any())
            {
                _currentGraph.Nodes.Add(new OutputNode());
            }
            else
            {
                while (_currentGraph.Nodes.Count > 1)
                {
                    _currentGraph.Nodes.RemoveAll((node) => node.GetType() == typeof(OutputNode));
                    _currentGraph.Nodes.Add(new OutputNode());
                }
            }

            return true;
        }

        private void DrawGraph()
        {
            _serializedObject = new SerializedObject(_currentGraph);
            _currentView = new PGGView(_serializedObject, this);
            _mainPreview = new PGGMainPreview(this);
            _currentView.graphViewChanged += OnChange;
            rootVisualElement.Add(_currentView);
            _currentView.Add(_mainPreview);
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            OnGraphChanged();

            return graphViewChange;
        }

        public void OnGraphChanged()
        {
            EditorUtility.SetDirty(_currentGraph);
            _currentGraph.Cook();

            _currentView.EditorNodes.ForEach((e) => e.DrawPreview());
        }
    }
}