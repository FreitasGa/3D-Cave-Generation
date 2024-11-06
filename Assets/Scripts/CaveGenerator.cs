using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CaveGenerator : MonoBehaviour
{
    private Graph _graph;

    private List<Vector3> _path = new();

    private Mesh _mesh;

    public GameObject nodes;

    [Range(.1f, 1f)]
    public float weight;

    [Range(1, 30)]
    public int k;

    [Range(1, 30)]
    public int spacer;

    [Range(.5f, 5f)]
    public float radius;

    [Range(3, 36)]
    public int segments;

    public bool autoUpdate;
    public bool generateMesh;

    private void OnDrawGizmos()
    {
        for (var i = 0; i < _path.Count; i++)
        {
            var current = _path[i];
            var next = _path[(i + 1) % _path.Count];

            Gizmos.color = Color.blue;

            if (_graph != null && _graph.Points.Any(p => p.Position == current))
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawSphere(current, 0.2f);

            if (i < _path.Count - 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(current, next);
            }
        }
        
        // for (var i = 0; i < _path.Count; i++)
        // {
        //     var current = _path[i];
        //     var next = _path[(i + 1) % _path.Count];
        //     var rotation = Quaternion.LookRotation(next - current);
        //
        //     for (var j = 0; j < segments; j++)
        //     {
        //         var angle = j * Mathf.PI * 2 / segments;
        //         var x = Mathf.Cos(angle);
        //         var y = Mathf.Sin(angle);
        //
        //         var point = new Vector3(x, y) * radius;
        //         point = rotation * point;
        //
        //         Gizmos.color = Color.cyan;
        //         Gizmos.DrawSphere(current + point, 0.1f);
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
        _path = PathGenerator.Generate(_graph, weight, k, spacer);
    }

    public void ClearPath()
    {
        _path.Clear();
    }

    public void GenerateMesh()
    {
        var meshData = MeshGenerator.Generate(_path, segments, radius);

        _mesh.SetVertices(meshData.Vertices);
        _mesh.SetTriangles(meshData.Triangles, 0);
        _mesh.RecalculateNormals();
    }

    public void ClearMesh()
    {
        _mesh.Clear();
    }
}