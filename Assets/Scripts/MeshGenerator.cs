using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData Generate(Graph graph, int segments, float radius)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        foreach (var point in graph.Points)
        {
            foreach (var path in point.Paths)
            {
                for (var i = 0; i < path.Count; i++)
                {
                    var current = path[i];
                    var next = path[(i + 1) % path.Count];

                    for (var j = 0; j < segments; j++)
                    {
                        var angle = j * Mathf.PI * 2 / segments;
                        var x = Mathf.Cos(angle);
                        var y = Mathf.Sin(angle);

                        var vertex = new Vector3(x, y) * radius;
                        vertex = Quaternion.LookRotation(next - current) * vertex;

                        vertices.Add(current + vertex);
                    }
                }
            }
        }

        for (var i = 0; i < graph.Points.Count; i++)
        {
            var point = graph.Points[i];
            var index = 0;
            
            if (i > 0)
            {
                index = triangles.Max() - segments + 1;
            }

            foreach (var path in point.Paths)
            {
                for (var k = 0; k < path.Count - 1; k++)
                {
                    for (var l = 0; l < segments; l++)
                    {
                        var a = index + l + k * segments;
                        var b = index + ((l + 1) % segments) + k * segments;
                        var c = index + l + ((k + 1) % path.Count) * segments;
                        var d = index + ((l + 1) % segments) + ((k + 1) % path.Count) * segments;

                        triangles.Add(a);
                        triangles.Add(d);
                        triangles.Add(c);

                        triangles.Add(a);
                        triangles.Add(b);
                        triangles.Add(d);
                    }
                }
            }
        }

        for (var i = 0; i < triangles.Count; i += 6)
        {
            Debug.Log(
                $"{triangles[i]}, {triangles[i + 1]}, {triangles[i + 2]}; {triangles[i + 3]}, {triangles[i + 4]}, {triangles[i + 5]}");
        }

        return new MeshData(vertices, triangles);
    }
}