using System;
using System.Collections.Generic;
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

    private void Awake()
    {
        Game.UI = this;
        CloseAll();
    }

    public void CloseAll()
    {
        foreach (ScreenUI screen in Screens)
            screen.Hide();
    }

    public void Show(string screenId)
    {
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

    internal void HideInteractable()
    {
        Hide("Interactable");
    }

    internal void ShowInteractable()
    {
        Show("Interactable");
    }
}
