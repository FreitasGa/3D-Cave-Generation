using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Linker : MonoBehaviour
{ 
    [SerializeField]
    public List<GameObject> edges;
    
    private void OnDrawGizmos()
    {
        foreach (var edge in edges.Where(edge => edge != null))
        {
            Gizmos.DrawLine(transform.position, edge.transform.position);
        }
    }
}
