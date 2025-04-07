using UnityEngine.Events;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public AudioSource AudioSource;
    public bool AudioSourceTakeOutFromParent = false;

    public UnityEvent OnStart;
    public UnityEvent OnUse;
    public bool Disposable = false;

    bool isUses = false;

    private void Awake()
    {
        isUses = false;
        OnStart?.Invoke();
        CustomAwake();
    }

    public virtual void CustomAwake() { }

    public void TryUse()
    {
        if (Disposable && isUses) return;
        Use();
    }

    public virtual void Use()
    {
        if (AudioSource)
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            if (AudioSourceTakeOutFromParent)
            {
                AudioSource.transform.parent = null;
            }
        }

        OnUse?.Invoke();
        if (Disposable)
        {
            isUses = true;
        }
    }
}


