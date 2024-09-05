using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform start;
    
    [SerializeField]
    private Transform waypoints;
    
    [SerializeField]
    [Range(0.05f, 0.95f)]
    private float weight;
    
    [SerializeField]
    [Range(1, 30)]
    private int multiplier = 5;
    
    private void OnDrawGizmos()
    {
        if (start == null || waypoints == null || waypoints.childCount == 0)
        {
            return;
        }
        
        foreach (Transform waypoint in waypoints)
        {
            var path = WeightedRandomWalk.Generate(start.position, waypoint.position, weight, multiplier);
            
            foreach (var point in path)
            {
                Gizmos.DrawSphere(point, 5f);
            }
        }
    }
}
