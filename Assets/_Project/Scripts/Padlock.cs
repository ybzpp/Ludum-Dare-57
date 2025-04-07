using UnityEngine;

public class Padlock : InteractableObject
{
    public string Key;

    public override void Use()
    {
        if (PlayerPrefs.GetInt(Key,0) == 0)
            return;

        if (Key == "Shovel")
        {
            AudioHelper.PlaySound("Shovel", transform.position);
        }

        base.Use();
        Destroy(gameObject);
    }
}
