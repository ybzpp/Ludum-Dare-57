using System.Linq;
using UnityEngine;

public class Note : InteractableObject
{
    public string Id;
    public override void Use()
    {
        base.Use();

        var note = Game.NoteManager.noteConfig.notes.Where(x => x.Id == Id).First();
        if (note != null)
        {
            Game.UI.ShowNote(Helper.GetCurretLocalization(note.Localizations));
        }
        else 
        {
            Debug.LogError($"Note id:{Id} not find!");
        }

        AudioHelper.PlaySound("Notes");
    }
}
