using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathGenerator
{
    public static List<Vector3> Generate(Graph graph, float weight, int k, int spacer)
    {
        var path = new List<Vector3>();

        foreach (var point in graph.Points)
        {
            foreach (var edge in point.Edges)
            {
                var points = WeightedRandomWalk.Generate(point.Position, edge.Position, weight, k, spacer);
                path.AddRange(points);
            }
        }

        path = new HashSet<Vector3>(path).ToList();
        
        return path;
    }
}