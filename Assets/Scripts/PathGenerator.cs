using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject nodes;
    
    [Range(0.05f, 0.95f)]
    public float weight;

    [Range(1, 30)] 
    public int spacer;
    
    public bool autoUpdate;
    
    private List<GameObject> _waypoints = new();
    private List<List<GameObject>> _edges = new();
    private List<HashSet<Vector3>> _paths = new();

    private void Awake()
    {
        var waypoints = new List<GameObject>();
        
        foreach (Transform child in nodes.transform)
        {
            waypoints.Add(child.GameObject());
        }
        
        var edges = new List<List<GameObject>>();
        
        foreach (var waypoint in waypoints)
        {
            var links = waypoint.GetComponent<Linker>().edges;
            edges.Add(links);
        }
        
        _waypoints = waypoints;
        _edges = edges;
    }

    private void OnDrawGizmos()
    {
        foreach (var path in _paths)
        {
            var previous = path.First();
            
            foreach (var current in path.Skip(1))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(current, 0.5f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previous, current);
                previous = current;
            }
        }
    }

    public void GeneratePath()
    {
        var paths = new List<HashSet<Vector3>>();
        
        foreach (var waypoint in _waypoints)
        {
            var edges = _edges[_waypoints.IndexOf(waypoint)];
            
            foreach (var edge in edges)
            {
                var path = WeightedRandomWalk.Generate(waypoint.transform.position, edge.transform.position, weight, spacer);
                paths.Add(path);
            }
        }
        
        _paths = paths;
    }
}
