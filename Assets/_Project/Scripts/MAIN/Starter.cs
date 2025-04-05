using UnityEngine;

public class Starter : MonoBehaviour
{
    public KeyCode VisibleCursorKeyCode;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(VisibleCursorKeyCode))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
