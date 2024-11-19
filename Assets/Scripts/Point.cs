using System;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public string ID;
    public Vector3 Position;
    public readonly List<Point> Edges;
    public readonly List<List<Vector3>> Paths;

    public Point(string id, Vector3 position)
    {
        ID = id;
        Position = position;
        Edges = new List<Point>();
        Paths = new List<List<Vector3>>();
    }
}