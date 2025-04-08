using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Menu,
    Game,
    Win,
}

public static class Helper
{
    public static Vector3 OnlyXZ(this Vector3 vector3)
    {
        vector3.y = 0f;
        return vector3;
    }

    public static void DestroyAllChilds(this Transform t)
    {
        foreach (Transform child in t)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    internal static string GetCurretLocalization(NotesData.LocalizationData[] localizations)
    {
        var currentLocale = PlayerPrefs.GetString("Locale", "en");

        foreach (var item in localizations)
        {
            if (item.LocaleId == currentLocale)
                return item.Text;
        }

        return localizations[0].Text;
    }
}

public static class Game
{
    public static UI UI;
    public static TransitionUI TransitionUI;
    public static SceneData SceneData;
    public static RuntimeData RuntimeData;
    public static PlayerController Player;
    public static DragRigidbody DragRigidbody;
    public static AudioManager AudioManager;
    public static NoteManager NoteManager;
    public static Reactor Reactor;

    public static void ChangeGameState(GameState state)
    {
        UI.CloseAll();
        RuntimeData.GameState = state;
        switch (state)
        {
            case GameState.None:
                InputUnlock();
                break;
            case GameState.Menu:
                InputUnlock();
                UI.Show("Menu");
                break;
            case GameState.Game:
                InputLock();
                UI.Show("Game");
                break;
            case GameState.Win:
                InputUnlock();
                RuntimeData.EndTime = Time.time;
                RuntimeData.GameTime = Time.time - RuntimeData.StartTime;
                UI.Show("Win");
                break;
            default:
                break;
        }
    }

    public static void InputLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void InputUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void Pause()
    {
        if (RuntimeData)
            RuntimeData.IsPause = true;

        InputUnlock();
    }

    public static void End()
    {
        if (RuntimeData)
            RuntimeData.IsEnd = true;

        UI.CloseAll();
        InputUnlock();
    }

    public static void Continue()
    {
        if (RuntimeData)
            RuntimeData.IsPause = false;

        InputLock();    
    }

    public static void StartGame()
    {
        UI.CloseAll();
        Reactor.OnStartGame?.Invoke();
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}