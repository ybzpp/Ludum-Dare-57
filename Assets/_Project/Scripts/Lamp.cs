using UnityEngine;

public class Lamp : MonoBehaviour
{
    public GameObject DisableView;
    public GameObject EnableView;
    public FlickerEffect FlickerEffect;

    public void Enable()
    {
        Debug.Log($"Lamp {gameObject.name} Enable");

        DisableView.SetActive(false);
        EnableView.SetActive(true);
    }

    public void Disable()
    {
        Debug.Log($"Lamp {gameObject.name} Disable");

        DisableView.SetActive(true);
        EnableView.SetActive(false);
    }

    public void DisableFlicker()
    {
        FlickerEffect.Disable();
    }

    public void EnableFlicker()
    {
        FlickerEffect.Enable();
    }
}
