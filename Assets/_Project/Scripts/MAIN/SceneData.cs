using UnityEngine;

public class SceneData : MonoBehaviour
{
    public GameObject Cutscene;
    public GameObject Padik;
    public GameObject WindSFX;
    public Material OutlineMaterial; //remove

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
        Game.RuntimeData.StartTime = Time.time;
    }

    public void ShowCutscene()
    {
        Cutscene.SetActive(true);
        WindSFX.SetActive(true);
        Padik.SetActive(false);
    }
}
