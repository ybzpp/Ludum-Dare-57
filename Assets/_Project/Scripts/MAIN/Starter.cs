using UnityEngine;

public class Starter : MonoBehaviour
{
    public GameState StartGameState;

    private void Start()
    {
        ApplySettings();
        Game.ChangeGameState(StartGameState);
    }

    public void ApplySettings()
    {

    }
}
