using UnityEngine;

public class SceneData : MonoBehaviour
{
    public GameObject Cutscene;
    public GameObject Padik;

    private void Awake()
    {
        Game.SceneData = this;
    }

    public void StartGame()
    {
        Cutscene.SetActive(false);
        Padik.SetActive(true);
        Game.ChangeGameState(GameState.Game);
    }
}
