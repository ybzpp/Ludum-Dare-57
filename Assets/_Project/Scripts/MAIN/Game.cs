using UnityEngine;

public enum GameState
{
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

    public static void ChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Game:
                break;
            default:
                break;
        }
    }
}