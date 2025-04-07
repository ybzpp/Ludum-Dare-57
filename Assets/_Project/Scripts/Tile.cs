using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SurfaceType SurfaceType;
    public string Id;
    public List<Transform> connectionPoints;
    public Transform endPoint => connectionPoints[1];
    public Transform startPoint => connectionPoints[0];

    private void Start()
    {
        var collider = GetComponentInChildren<Collider>();
        if (collider != null) 
            collider.gameObject.AddComponent<Surface>().SurfaceType = SurfaceType;
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(Id))
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
