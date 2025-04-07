using UnityEngine;

public class PickableItem : InteractableObject
{
    public string Id;
    public override void Use()
    {
        base.Use();

        PlayerPrefs.SetInt(Id, 1);
        gameObject.SetActive(false);
    }
}
