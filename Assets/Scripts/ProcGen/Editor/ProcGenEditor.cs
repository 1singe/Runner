using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(ProceduralGenerationManager))]
public class ProcGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralGenerationManager generator = (ProceduralGenerationManager)target;

        if (GUILayout.Button("Apply"))
        {
            generator.Regenerate();
        }

        base.OnInspectorGUI();

        if (GUILayout.Button("Generate From Biome Random"))
        {
            int seed = (int)DateTime.Now.Ticks;
            generator.Editor_GenerateSingleChunk(seed);
            Debug.Log(seed);
        }

        if (GUILayout.Button("Generate From Biome Seeded"))
        {
            generator.Editor_GenerateSingleChunk(generator.PredefinedSeed);
        }

        if (GUILayout.Button("Destroy"))
        {
            generator.Editor_DestroyChunk();
        }
    }
}