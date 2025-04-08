using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Transform Target;

    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!Target)
            return;

        lineRenderer.SetPositions(new Vector3[]
        {
            transform.position,
            Target.position,
        });
    }
}
