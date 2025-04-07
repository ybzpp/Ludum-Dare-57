using System;
using TMPro;
using UnityEngine.UI;

public class NotesScreen : ScreenUI
{
    public TMP_Text NoteText;
    public Button BackButton;

    private void OnEnable()
    {
        BackButton.onClick.AddListener(Back);
    }

    private void OnDisable()
    {
        BackButton.onClick.RemoveListener(Back);
    }

    private void Back()
    {
        Hide();
    }

    public void SetText(string text)
    {
        NoteText.text = text;
    }

    public override void Hide()
    {
        base.Hide();
        Game.Continue();
    }

    public override void Show()
    {
        base.Show();
        Game.Pause();
    }
}