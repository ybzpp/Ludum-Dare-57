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

public class ScreenUI : ElementUI
{
    public string Id;
}

public class UI : MonoBehaviour
{
    public List<ScreenUI> Screens;

    private void Awake()
    {
        Game.UI = this;
    }

    public void CloseAll()
    {
        foreach (ScreenUI screen in Screens)
            screen.Hide();
    }

    public void Show(string screenId)
    {
        CloseAll();
        foreach (var item in Screens)
        {
            if (item.Id == screenId)
            {
                item.Show();
                break;
            }    
        }
    }
}
