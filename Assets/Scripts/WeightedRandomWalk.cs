using System.Collections.Generic;
using UnityEngine;

public static class WeightedRandomWalk
{
    public static HashSet<Vector3> Generate(Vector3 start, Vector3 end, float weight, int multiplier)
    {
        var path = new HashSet<Vector3> { start };
        var current = start;
        
        while (Vector3.Distance(current, end) > multiplier)
        {
            var next = current + WeightedRandomMove(end - current, weight, multiplier);
            path.Add(next);
            
            current = next;
        }
        
        return path;
    }
    
    private static Vector3 WeightedRandomMove(Vector3 direction, float weight, int multiplier)
    {
        var random = Random.Range(0f, 1f);
        
        if (random < weight)
        {
            return direction.normalized * multiplier;
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
        } * multiplier;
    }
}
