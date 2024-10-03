using System.Collections.Generic;
using UnityEngine;

public static class PathGenerator
{
    public static List<List<Vector3>> Generate(Graph graph, float weight, int spacer)
    {
        var paths = new List<List<Vector3>>();
        
        foreach (var point in graph.Points)
        {
            var edges = point.Edges;
            
            foreach (var edge in edges)
            {
                var path = WeightedRandomWalk.Generate(point.Position, edge.Position, weight, spacer);
                paths.Add(path);
            }
        }
        
        return paths;
    }
}
