using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CaveGenerator))]
public class CaveGeneratorEditor : Editor
{
    private CaveGenerator _generator;
    
    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            if (_generator.autoUpdate)
            {
                _generator.GeneratePath();
                
                if (_generator.generateMesh)
                {
                    _generator.GenerateMesh();
                }
            }
        }

        if (GUILayout.Button("Generate"))
        {   
            _generator.GeneratePath();
            
            if (_generator.generateMesh)
            {
                _generator.GenerateMesh();
            }
        }
        
        if (GUILayout.Button("Clear"))
        {
            _generator.ClearPath();
            _generator.ClearMesh();
        }
    }

    public void OnEnable()
    {
        _generator = (CaveGenerator)target;
        _generator.Load();
    }
}
