using System.Collections;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject PreEndView;
    public GameObject EndView;

    public Transform StartPoint;
    public Transform End;
    public float FlyToStartTime = 1f;
    public float FlyToEndTime = 10f;


    private void Start()
    {
        PreEndView.SetActive(true);
        EndView.SetActive(false);
    }

    public void ShowEnd()
    {
        PreEndView.SetActive(false);
        EndView.SetActive(true);
    }

    public void GoEnd()
    {
        Game.Pause();
        Game.UI.gameObject.SetActive(false);

        StartCoroutine(CameraAnimation(Game.Player.PlayerCamera));
    }

    public IEnumerator CameraAnimation(Camera camera)
    {
        AudioHelper.PlaySound("EndSwoosh");

        var t = 0f;
        camera.transform.parent = null;
        var startPos = camera.transform.position;
        var startDir = camera.transform.forward;
        while (t < FlyToStartTime)
        {
            camera.transform.position = Vector3.Lerp(startPos, StartPoint.position, t / FlyToStartTime);
            camera.transform.forward = Vector3.Lerp(startDir, StartPoint.forward, t / FlyToStartTime);
            t += Time.deltaTime;
            yield return null;
        }

        Game.TransitionUI.ShowWhiteScreen();
        Game.TransitionUI.FadeIn(() =>
        {
            Game.ChangeGameState(GameState.Win);
            Game.TransitionUI.FadeOut();
        });

        t = 0;
        var currentPos = camera.transform.position;
        while (t < FlyToEndTime)
        {
            camera.transform.position = Vector3.Lerp(StartPoint.position, End.position, t / FlyToEndTime);
            camera.transform.forward = StartPoint.forward;
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
