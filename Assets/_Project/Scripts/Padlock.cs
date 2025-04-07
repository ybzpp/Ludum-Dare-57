using UnityEngine;

public class Padlock : InteractableObject
{
    public string Key;

    public override void Use()
    {
        if (PlayerPrefs.GetInt(Key,0) == 0)
            return;

        base.Use();
        Destroy(gameObject);
    }
}
