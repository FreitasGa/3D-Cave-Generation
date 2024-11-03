using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 Position;
    public readonly List<Point> Edges;

    public Point(Vector3 position)
    {
        Position = position;
        Edges = new List<Point>();
    }
}