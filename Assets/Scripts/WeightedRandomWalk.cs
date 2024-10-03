using System.Collections.Generic;
using UnityEngine;

public static class WeightedRandomWalk
{
    public static List<Vector3> Generate(Vector3 start, Vector3 end, float weight, int spacer)
    {
        var path = new List<Vector3> { start };
        var current = start;
        
        while (Vector3.Distance(current, end) > spacer)
        {
            var next = current + WeightedRandomMove(end - current, weight, spacer);
            path.Add(next);
            
            current = next;
        }
        
        return path;
    }
    
    private static Vector3 WeightedRandomMove(Vector3 direction, float weight, int spacer)
    {
        var random = Random.Range(0f, 1f);
        
        if (random < weight)
        {
            return direction.normalized * spacer;
        }
        
        var randomDirection = Random.Range(0, 6);
        
        return randomDirection switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,
            4 => Vector3.forward,
            5 => Vector3.back,
            _ => Vector3.zero
        } * spacer;
    }
}
