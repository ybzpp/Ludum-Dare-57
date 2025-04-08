using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementUI : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

public class UI : MonoBehaviour
{
    public List<ScreenUI> Screens;
    public NotesScreen Notes;
    public TextTyper ToBeContinuedTextTyper;

    private void Awake()
    {
        Game.UI = this;

        CloseAll();
        GetComponent<CanvasGroup>().alpha = 1;

        var n = GetScreen("Notes"); 
        if (n != null)
        {
            Notes = n.GetComponent<NotesScreen>();
        }
        else
        {
            Debug.LogError($"NotesScreen not find!");
        }
    }

    public void CloseAll() 
    {
        foreach (ScreenUI screen in Screens)
            screen.Hide();
    }

    public void Show(string screenId)
    {
        gameObject.SetActive(true);
        foreach (var item in Screens)
        {
            if (item.Id == screenId)
            {
                item.Show();
                break;
            }    
        }
    }
    
    public void Hide(string screenId)
    {
        foreach (var item in Screens)
        {
            if (item.Id == screenId)
            {
                item.Hide();
                break;
            }    
        }
    }

    public ScreenUI GetScreen(string id)
    {
        foreach (var item in Screens)
        {
            if (item.Id == id)
            {
                return item;
            }    
        }

        return null;
    }

    internal void HideInteractable()
    {
        Hide("Interactable");
        Show("Cursor");
    }

    internal void ShowInteractable()
    {
        Show("Interactable");
        Hide("Cursor");
    }

    internal void ShowNote(string text)
    {
        Notes.SetText(text);
        Notes.Show();
    }
}
