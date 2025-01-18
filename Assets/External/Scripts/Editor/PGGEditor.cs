using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace PGG.Editor
{
    [CustomEditor(typeof(GraphAsset))]
    public class PGGEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset.GetType() == typeof(GraphAsset))
            {
                PGGWindow.Open((GraphAsset)asset);
                return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                PGGWindow.Open((GraphAsset)target);
            }
        }
    }
}