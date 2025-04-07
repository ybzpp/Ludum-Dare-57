using Unity.VisualScripting;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    [Header("Drag Settings")]
    public float maxDistance = 5f;
    public float dragForce = 10f;
    public LayerMask draggableLayers;
    public bool IsDraggable => grabbedRigidbody != null;

    private Camera cam;
    private Rigidbody grabbedRigidbody;
    private Vector3 grabOffset;
    private float grabDistance;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        Game.DragRigidbody = this;
    }

    private int _draggbleLayerMaskIndex = 11;
    void Update()
    {
        if (Game.RuntimeData.IsPause)
            return;

        // On mouse down — try to grab
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }

        // While mouse held — drag
        if (Input.GetMouseButton(0) && grabbedRigidbody != null)
        {
            DragObject();
        }

        if (grabbedRigidbody != null && grabbedRigidbody.gameObject.layer != _draggbleLayerMaskIndex) 
        {   
            grabbedRigidbody = null;
        }

        // On mouse up — release
        if (Input.GetMouseButtonUp(0) && grabbedRigidbody != null)
        {
            ReleaseObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, draggableLayers))
        {
            if (hit.rigidbody != null)
            {
                if (!hit.rigidbody.isKinematic && !hit.rigidbody.CompareTag("Fuse"))
                {
                    return;
                }

                hit.rigidbody.isKinematic = false;
                grabbedRigidbody = hit.rigidbody;
                grabOffset = hit.point - grabbedRigidbody.transform.position;
                grabDistance = hit.distance;
                grabbedRigidbody.useGravity = false; // optional: disable gravity while dragging
            }
        }
    }

    void DragObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(grabDistance) - grabOffset;
        Vector3 newPos = Vector3.Lerp(grabbedRigidbody.position, targetPoint, Time.deltaTime * dragForce);
        grabbedRigidbody.MovePosition(newPos);
    }

    void ReleaseObject()
    {
        grabbedRigidbody.useGravity = true;
        grabbedRigidbody = null;
    }
}

