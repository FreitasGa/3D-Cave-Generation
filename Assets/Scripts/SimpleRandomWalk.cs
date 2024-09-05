using System.Collections.Generic;
using UnityEngine;

public static class SimpleRandomWalk
{
    public static HashSet<Vector3> Generate(Vector3 start, int steps)
    {
        var path = new HashSet<Vector3> { start };
        var current = start;
        
        for (var i = 0; i < steps; i++)
        {
            var next = current + RandomMove();
            path.Add(next);
            
            current = next;
        }
        
        return path;
    }

    private static Vector3 RandomMove()
    {
        var random = Random.Range(0, 6);

        return random switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,
            4 => Vector3.forward,
            5 => Vector3.back,
            _ => Vector3.zero
        };
    }
}
