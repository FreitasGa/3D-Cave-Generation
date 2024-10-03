using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CaveGenerator : MonoBehaviour
{
    [HideInInspector]
    public Graph Graph;

    [HideInInspector] 
    public List<List<Vector3>> Paths = new ();

    [HideInInspector] 
    public Mesh mesh;
    
    public GameObject nodes;
    
    [Range(0.05f, 0.95f)]
    public float weight;

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
        foreach (var path in Paths)
        {
            var previous = path.First();
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(previous, 0.5f);
            
            foreach (var current in path.Skip(1))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(current, 0.5f);
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previous, current);
                
                previous = current;
            }
        }

        // foreach (var path in Paths)
        // {
        //     for (int i = 0; i < path.Count; i++)
        //     {
        //         var current = path[i];
        //         var next = path[(i + 1) % path.Count];
        //         
        //         var rotation = Quaternion.LookRotation(next - current);
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
        //             Gizmos.color = j switch
        //             {
        //                 0 => Color.red,
        //                 1 => Color.green,
        //                 2 => Color.blue,
        //                 3 => Color.yellow,
        //                 4 => Color.cyan,
        //                 5 => Color.magenta,
        //                 _ => Color.white
        //             };
        //             Gizmos.DrawSphere(current + point, 0.2f);
        //         }
        //     }
        // }
    }

    public void Load()
    {
        mesh = new Mesh();
        mesh.name = "Cave";
        
        GetComponent<MeshFilter>().sharedMesh = mesh;
        
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
        
        Graph = new Graph(points);
    }
    
    public void GeneratePath()
    {
        Paths = PathGenerator.Generate(Graph, weight, spacer);
    }
    
    public void ClearPath()
    {
        Paths.Clear();
    }

    public void GenerateMesh()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        
        foreach (var path in Paths)
        {
            for (int i = 0; i < path.Count; i++)
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

            for (int i = 0; i < path.Count - 1; i++)
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
        }
        
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }
    
    public void ClearMesh()
    {
        mesh.Clear();
    }
}
