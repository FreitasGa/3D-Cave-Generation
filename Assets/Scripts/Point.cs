using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 Position;
    public List<Point> Edges;

    public Point(Vector3 position)
    {
        this.Position = position;
        Edges = new List<Point>();
    }
}
