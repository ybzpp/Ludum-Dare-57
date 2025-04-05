using UnityEngine;

public class ParentTrigger : MonoBehaviour
{
    public Transform newParent;
    private Transform originalParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            originalParent = other.transform.parent;
            other.transform.SetParent(newParent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(originalParent);
        }
    }

    private void Reset()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
            collider = GetComponent<Collider>();
        }
        if (!collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Collider collider = GetComponent<Collider>();

        if (collider is BoxCollider)
        {
            BoxCollider boxCollider = (BoxCollider)collider;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
        else if (collider is SphereCollider)
        {
            SphereCollider sphereCollider = (SphereCollider)collider;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
        }
        else if (collider is CapsuleCollider)
        {
            CapsuleCollider capsuleCollider = (CapsuleCollider)collider;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Vector3 size = new Vector3(capsuleCollider.radius * 2, capsuleCollider.height / 2, capsuleCollider.radius * 2);
            Gizmos.DrawCube(capsuleCollider.center, size);
        }
    }
}