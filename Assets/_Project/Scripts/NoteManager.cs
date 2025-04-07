using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public NotesData noteConfig;

    private void Awake()
    {
        if (Game.NoteManager == null)
        {
            Game.NoteManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
