using UnityEngine;

public class SceneData : MonoBehaviour
{
    public GameObject Cutscene;
    public GameObject Padik;
    public GameObject WindSFX;

    private void Awake()
    {
        Game.SceneData = this;
        ShowCutscene();
    }

    public void StartGame()
    {
        Cutscene.SetActive(false);
        WindSFX.SetActive(false);
        Padik.SetActive(true);
        Game.ChangeGameState(GameState.Game);
        Game.TransitionUI.FadeOut();
    }

    public void ShowCutscene()
    {
        Cutscene.SetActive(true);
        WindSFX.SetActive(true);
        Padik.SetActive(false);
    }
}
