using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData Generate(List<Vector3> path, int segments, float radius)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        for (var i = 0; i < path.Count; i++)
        {
            var current = path[i];
            var next = path[(i + 1) % path.Count];
            var rotation = Quaternion.LookRotation(next - current);

            for (var j = 0; j < segments; j++)
            {
                var angle = j * Mathf.PI * 2 / segments;
                var x = Mathf.Cos(angle);
                var y = Mathf.Sin(angle);

                var point = new Vector3(x, y) * radius;
                point = rotation * point;

                vertices.Add(current + point);
            }
        }

        for (var i = 0; i < path.Count - 1; i++)
        {
            for (var j = 0; j < segments; j++)
            {
                var a = (j + i * segments);
                var b = (a + 1);
                var c = (a + segments);
                var d = (a + segments + 1);

                if (b % segments == 0)
                {
                    b -= segments;
                    d -= segments;
                }

                triangles.Add(a);
                triangles.Add(d);
                triangles.Add(c);

                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(d);
            }
        }

        return new MeshData(vertices, triangles);
    }
}