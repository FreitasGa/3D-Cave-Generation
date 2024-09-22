using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathGenerator))]
public class PathGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PathGenerator generator = (PathGenerator)target;

        if (DrawDefaultInspector())
        {
            if (generator.autoUpdate)
            {
                generator.GeneratePath();
            }
        }

        if (GUILayout.Button("Generate"))
        {   
            generator.GeneratePath();
        }
    }
}
