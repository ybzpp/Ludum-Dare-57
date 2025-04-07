using UnityEngine;

public class PickableItem : InteractableObject
{
    public string Id;
    public override void Use()
    {
        base.Use();

        switch (Id)
        {
            case "Revolver":
                AudioHelper.PlaySound("PickUpGun");
                break;
            case "RedKey":
                AudioHelper.PlaySound("PickUpKey");
                break;
            default:
                break;
        }

        PlayerPrefs.SetInt(Id, 1);
        gameObject.SetActive(false);
    }
}
