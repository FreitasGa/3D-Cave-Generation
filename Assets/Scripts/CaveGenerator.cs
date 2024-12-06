using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CaveGenerator : MonoBehaviour
{
    private Graph _graph;

    private Mesh _mesh;

    public GameObject nodes;

    [Range(.1f, 1f)]
    public float weight;

    [Range(1, 30)]
    public int k;

    [Range(1, 30)]
    public int spacer;

    [Range(1f, 10f)]
    public float radius;

    [Range(6, 12)]
    public int segments;

    public bool autoUpdate;
    public bool generateMesh;

    private void OnDrawGizmos()
    {
        if (_graph == null)
        {
            return;
        }

        foreach (var point in _graph.Points)
        {
            foreach (var path in point.Paths)
            {
                for (var i = 0; i < path.Count; i++)
                {
                    var current = path[i];
                    var next = path[(i + 1) % path.Count];
        
                    Gizmos.color = Color.blue;
        
                    if (_graph != null && _graph.Points.Any(p => p.Position == current))
                    {
                        Gizmos.color = Color.green;
                    }
        
                    Gizmos.DrawSphere(current, 1f);
        
                    if (i < path.Count - 1)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(current, next);
                    }
                }
            }
        }
    }

    private bool ValidateLoad()
    {
        if (_graph == null)
        {
            return true;
        }

        if (nodes.transform.childCount != _graph.Points.Count)
        {
            return true;
        }

        if (nodes
            .transform
            .Cast<Transform>()
            .Any(child => _graph.Points.All(point => point.Position != child.position)))
        {
            return true;
        }

        return false;
    }

    public void Load()
    {
        if (ValidateLoad() == false)
        {
            return;
        }

        var waypoints = new List<GameObject>();
        var points = new List<Point>();

        foreach (Transform child in nodes.transform)
        {
            waypoints.Add(child.GameObject());

            var point = new Point(child.name, child.position);
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
        _mesh = new Mesh { name = "Cave" };

        GetComponent<MeshFilter>().sharedMesh = _mesh;
    }

    public void GeneratePath()
    {
        PathGenerator.Generate(_graph, weight, k, spacer);
    }

    public void ClearPath()
    {
        _graph.Points.ForEach(point => point.Paths.Clear());
    }

    public void GenerateMesh()
    {
        var meshData = MeshGenerator.Generate(_graph, segments, radius);

        _mesh.SetVertices(meshData.Vertices);
        _mesh.SetTriangles(meshData.Triangles, 0);
        _mesh.RecalculateNormals();
    }

    public void ClearMesh()
    {
        _mesh.Clear();
    }
}