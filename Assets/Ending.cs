using System.Collections;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject PreEndView;
    public GameObject EndView;

    public Transform Start;
    public Transform End;
    public float FlyToStartTime = 1f;
    public float FlyToEndTime = 10f;

    public void ShowEnd()
    {
        PreEndView.SetActive(false);
        EndView.SetActive(true);
    }

    public void GoEnd()
    {
        Game.RuntimeData.IsPause = true;
        Game.UI.CloseAll();

        StartCoroutine(CameraAnimation(Game.Player.PlayerCamera));
    }

    public IEnumerator CameraAnimation(Camera camera)
    {
        var t = 0f;
        camera.transform.parent = null;
        var startPos = camera.transform.position;
        var startDir = camera.transform.forward;
        while (t < FlyToStartTime)
        {
            camera.transform.position = Vector3.Lerp(startPos, Start.position, t / FlyToStartTime);
            camera.transform.forward = Vector3.Lerp(startDir, Start.forward, t / FlyToStartTime);
            t += Time.deltaTime;
            yield return null;
        }

        Game.TransitionUI.ShowWhiteScreen();
        Game.TransitionUI.FadeIn(() => Game.Win());


        t = 0;
        var currentPos = camera.transform.position;
        while (t < FlyToEndTime)
        {
            camera.transform.position = Vector3.Lerp(currentPos, End.position, t / FlyToEndTime);
            t += Time.deltaTime;
            yield return null;
        }

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
