using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class WeightedRandomWalk
{
    public static List<Vector3> Generate(Vector3 start, Vector3 end, float weight, int k, int spacer)
    {
        var path = new List<Vector3> { start };
        var current = start;

        var moves = MovesWithHalfSteps();

        while (Vector3.Distance(current, end) > spacer)
        {
            var next = current + WeightedRandomMove(end - current, weight, k, spacer, moves);
            path.Add(next);

            current = next;
        }

        if (current != end)
        {
            path.Add(end);
        }

        for (var i = 0; i < path.Count; i++)
        {
            current = path[i];

            for (var j = i + 1; j < path.Count; j++)
            {
                var next = path[j];

                if (current == next)
                {
                    path.RemoveRange(i, j - i);
                }
            }
        }

        for (var i = 0; i < path.Count; i++)
        {
            current = path[i];

            for (var j = i + 1; j < path.Count; j++)
            {
                var next = path[j];

                if (current == next)
                {
                    path.RemoveRange(i, j - i);
                }
            }
        }

        return path;
    }

    private static List<Vector3> MovesWithHalfSteps()
    {
        float[] steps = { -1f, -.5f, 0f, .5f, 1f };
        var moves = new List<Vector3>();

        foreach (var x in steps)
        {
            foreach (var y in steps)
            {
                foreach (var z in steps)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }

                    moves.Add(new Vector3(x, y, z));
                }
            }
        }

        return moves;
    }

    private static Vector3 WeightedRandomMove(Vector3 direction, float weight, int k, int spacer, List<Vector3> moves)
    {
        var affinities = new List<float>(moves.Count);

        foreach (var move in moves)
        {
            var affinity = Vector3.Dot(direction.normalized, move.normalized);
            affinities.Add(affinity);
        }

        var probabilities = new List<float>(moves.Count);

        for (var i = 0; i < moves.Count; i++)
        {
            var affinity = affinities[i];
            var probability = Mathf.Exp(affinity * weight * k);
            probabilities.Add(probability);
        }

        var probabilitySum = probabilities.Sum();

        for (var i = 0; i < probabilities.Count; i++)
        {
            probabilities[i] /= probabilitySum;
        }

        var probabilityMax = 0f;
        var random = Random.value;

        for (var i = 0; i < probabilities.Count; i++)
        {
            probabilityMax += probabilities[i];

            if (random <= probabilityMax)
            {
                return moves[i] * spacer;
            }
        }

        return Vector3.zero;
    }
}