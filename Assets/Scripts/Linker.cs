using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour
{
    public List<GameObject> edges;

    private void OnDrawGizmos()
    {
        foreach (var edge in edges)
        {
            if (edge == null)
            {
                continue;
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, edge.transform.position);
        }
    }
}