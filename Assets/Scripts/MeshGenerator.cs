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
                    var next = i < path.Count - 1 ? path[i + 1] : path[i];

                    var direction = next - current;
                    if (direction == Vector3.zero)
                    {
                        direction = Vector3.forward;
                    }

                    direction.Normalize();

                    var rotation = Quaternion.LookRotation(direction, Vector3.up);

                    for (var j = 0; j < segments; j++)
                    {
                        var angle = j * Mathf.PI * 2 / segments;
                        var x = Mathf.Cos(angle);
                        var y = Mathf.Sin(angle);
                        var vertex = rotation * new Vector3(x, y, 0) * radius;

                        vertices.Add(current + vertex);
                    }
                }
            }
        }

        var max = 0;
        foreach (var point in graph.Points)
        {
            foreach (var path in point.Paths)
            {
                for (var k = 0; k < path.Count - 1; k++)
                {
                    for (var l = 0; l < segments; l++)
                    {
                        var a = max + l + k * segments;
                        var b = max + (l + 1) % segments + k * segments;
                        var c = max + l + (k + 1) % path.Count * segments;
                        var d = max + (l + 1) % segments + (k + 1) % path.Count * segments;

                        triangles.Add(a);
                        triangles.Add(d);
                        triangles.Add(c);

                        triangles.Add(a);
                        triangles.Add(b);
                        triangles.Add(d);
                    }
                }

                max += path.Count * segments;
            }
        }

        return new MeshData(vertices, triangles);
    }
}