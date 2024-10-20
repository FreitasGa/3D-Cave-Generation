using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<Point> Points;

    public Graph(List<Point> points)
    {
        this.Points = points;
    }

    public void AddPoint(Point point)
    {
        Points.Add(point);
    }

    public void RemovePoint(Point point)
    {
        Points.Remove(point);
    }

    public void MovePoint(int index, Vector3 position)
    {
        Points[index].Position = position;
    }
}