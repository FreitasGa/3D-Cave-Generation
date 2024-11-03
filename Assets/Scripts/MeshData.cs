using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public readonly List<Vector3> Vertices;
    public readonly List<int> Triangles;

    public MeshData(List<Vector3> vertices, List<int> triangles)
    {
        Vertices = vertices;
        Triangles = triangles;
    }
}