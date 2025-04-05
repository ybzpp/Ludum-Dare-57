

public enum GameState
{
    Menu,
    Game,
}

public static class Game
{
    public static UI UI;
    public static SceneData SceneData;
    public static RuntimeData RuntimeData;


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