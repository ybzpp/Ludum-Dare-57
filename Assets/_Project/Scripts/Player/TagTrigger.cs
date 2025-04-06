using UnityEngine;
using UnityEngine.Events;

public class TagTrigger : MonoBehaviour
{
    public string Tag;
    public UnityEvent OnTrigger;
    public UnityEvent<Collider> OnColliderTrigger;
    public Collider Collider;
    public bool Disposable = true;

    private void Awake()
    {
        if (!Collider) Collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(Tag))
        {
            return;
        }

        if (other.CompareTag(Tag))
        {
            Trigger(other);
        }
    }

    public void Trigger(Collider other)
    {
        OnTrigger?.Invoke();
        OnColliderTrigger?.Invoke(other);
        if (Disposable) Collider.enabled = false;
    }
}
