using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform followpoint;
    void Start()
    {
        
    }

    void Update()
    {
        lineRenderer.SetPosition(1, followpoint.position);
    }
}
