using UnityEngine;
using UnityEngine.EventSystems;

public class FootstepSound : MonoBehaviour
{
    public LayerMask groundLayers;
    public float raycastDistance = 0.5f;
    public AudioSource audioSource;

    private SurfaceType CheckSurface()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayers))
        {
            Debug.Log($"hit:{hit.collider.name}");
            if (hit.collider.TryGetComponent(out Surface surface))
            {
                return surface.SurfaceType;
            }
        }

        return SurfaceType.None;
    }

    public void PlayFootstepSound()
    {

        Debug.Log($"PlayFootstepSound");    

        switch (CheckSurface())
        {
            case SurfaceType.None:
                break;
            case SurfaceType.Wood:
                AudioHelper.PlaySound("Wood_Walk", audioSource);
                break;
            case SurfaceType.Metal:
                AudioHelper.PlaySound("Metal_Walk", audioSource);
                break;
            case SurfaceType.Rock:
                AudioHelper.PlaySound("Rock_Walk", audioSource);
                break;
            case SurfaceType.Tile:
                AudioHelper.PlaySound("Tile_Walk", audioSource);
                break;
            case SurfaceType.Snow:
                AudioHelper.PlaySound("Snow_Walk", audioSource);
                break;
            default:
                break;
        }
    }
}