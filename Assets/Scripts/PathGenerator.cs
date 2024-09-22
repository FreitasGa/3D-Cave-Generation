using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject waypoints;
    
    [SerializeField]
    private GameObject start;
    
    [SerializeField]
    [Range(0.05f, 0.95f)]
    private float weight;
    
    [SerializeField]
    [Range(1, 30)]
    private int multiplier = 5;
    
    private List<GameObject> _edges = new();
    private List<GameObject> _waypoints = new();
    private List<List<GameObject>> _waypointEdges = new();
    
    private List<HashSet<Vector3>> _paths = new();
    
    private void Start()
    {
        _edges = start.GetComponent<Linker>().edges;

        foreach (Transform child in waypoints.transform)
        {
            var linker = child.GetComponent<Linker>();
            
            _waypoints.Add(child.GameObject());
            _waypointEdges.Add(linker.edges);
        }
        
        GeneratePath();
    }
    
    private void OnDrawGizmos()
    {
        foreach (var path in _paths)
        {
            var previous = path.First();
            
            foreach (var current in path.Skip(1))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previous, current);
                previous = current;
            }
        }
    }

    private void GeneratePath()
    {
        foreach (var edge in _edges)
        {
            var path = WeightedRandomWalk.Generate(start.transform.position, edge.transform.position, weight,
                multiplier);
            _paths.Add(path);
        }

        foreach (var waypoint in _waypoints)
        {
            var edges = _waypointEdges[_waypoints.IndexOf(waypoint)];

            foreach (var edge in edges)
            {
                var path = WeightedRandomWalk.Generate(waypoint.transform.position, edge.transform.position, weight,
                    multiplier);
                _paths.Add(path);
            }
        }
    }
}
