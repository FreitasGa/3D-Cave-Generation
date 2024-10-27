using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CaveGenerator : MonoBehaviour
{
    private Graph _graph;

    private List<List<Vector3>> _paths = new();

    private Mesh _mesh;

    public GameObject nodes;

    [Range(.1f, 1f)]
    public float weight;

    [Range(1, 30)]
    public int k;

    [Range(1, 30)]
    public int spacer;

    [Range(1f, 5f)]
    public float radius;

    [Range(3, 36)]
    public int segments;

    public bool autoUpdate;
    public bool generateMesh;

    private void OnDrawGizmos()
    {
        foreach (var path in _paths)
        {
            for (var i = 0; i < path.Count; i++)
            {
                var current = path[i];
                var next = path[(i + 1) % path.Count];

                Gizmos.color = Color.blue;

                if (i == 0 || i == path.Count - 1)
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawSphere(current, 0.2f);

                if (i < path.Count - 1)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(current, next);
                }
            }
        }

        // foreach (var path in _paths)
        // {
        //     var lastRotation = Quaternion.identity;
        //     
        //     for (int i = 0; i < path.Count; i++)
        //     {
        //         var current = path[i];
        //         var next = path[(i + 1) % path.Count];
        //         
        //         var rotation = Quaternion.LookRotation(next - current);
        //         
        //         if (i == path.Count - 1)
        //         {
        //             rotation = lastRotation;
        //         }
        //         
        //         for (var j = 0; j < segments; j++)
        //         {
        //             var angle = j * Mathf.PI * 2 / segments;
        //             var x = Mathf.Cos(angle);
        //             var y = Mathf.Sin(angle);
        //
        //             var point = new Vector3(x, y) * radius;
        //             point = rotation * point;
        //
        //             Gizmos.color = Color.red;
        //             Gizmos.DrawSphere(current + point, 0.2f);
        //         }
        //     }
        // }
    }

    public void Load()
    {
        _mesh = new Mesh
        {
            name = "Cave"
        };

        GetComponent<MeshFilter>().sharedMesh = _mesh;

        var waypoints = new List<GameObject>();
        var points = new List<Point>();

        foreach (Transform child in nodes.transform)
        {
            waypoints.Add(child.GameObject());

            var point = new Point(child.position);
            points.Add(point);
        }

        for (var i = 0; i < waypoints.Count; i++)
        {
            var waypoint = waypoints[i];
            var point = points[i];

            var edges = waypoint.GetComponent<Linker>().edges;

            foreach (var edge in edges)
            {
                var index = waypoints.IndexOf(edge);
                var edgePoint = points[index];

                point.Edges.Add(edgePoint);
            }
        }

        _graph = new Graph(points);
    }

    public void GeneratePath()
    {
        _paths = PathGenerator.Generate(_graph, weight, k, spacer);
    }

    public void ClearPath()
    {
        _paths.Clear();
    }

    public void GenerateMesh()
    {
        var meshData = MeshGenerator.Generate(_paths, segments, radius);

        _mesh.SetVertices(meshData.Vertices);
        _mesh.SetTriangles(meshData.Triangles, 0);
        _mesh.RecalculateNormals();
    }

    public void ClearMesh()
    {
        _mesh.Clear();
    }
}