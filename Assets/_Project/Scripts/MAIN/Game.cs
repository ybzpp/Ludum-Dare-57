using System;
using UnityEngine;

public enum GameState
{
    None,
    Menu,
    Game,
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
}

public static class Game
{
    public static UI UI;
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
        RuntimeData.IsPause = true;
        InputUnlock();
    }

    public static void Continue()
    {
        RuntimeData.IsPause = false;
        InputLock();    
    }

    public static void StartGame()
    {
        UI.CloseAll();
        Reactor.OnStartGame?.Invoke();
    }
}