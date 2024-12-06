using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathGenerator
{
    public static void Generate(Graph graph, float weight, int k, int spacer)
    {
        foreach (var point in graph.Points)
        {
            point.Paths.Clear();

            foreach (var edge in point.Edges)
            {
                var path = WeightedRandomWalk.Generate(point.Position, edge.Position, weight, k, spacer);
                point.Paths.Add(path);
            }
        }
    }
}