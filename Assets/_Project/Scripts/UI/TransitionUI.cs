using System.Collections;
using UnityEngine;

public class TransitionUI : ScreenUI
{
    public GameObject BlackScreen;
    public GameObject WhiteScreen;
    public float FadeTime = 1f;
    public float FadeInTime = 3f;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (Game.TransitionUI == null)
        {
            Game.TransitionUI = this;
            DontDestroyOnLoad(gameObject);
            ShowWhiteScreen();
            FadeOut();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CloseAll()
    {
        BlackScreen.SetActive(false);
        WhiteScreen.SetActive(false);
    }

    public void ShowBlackScreen()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        WhiteScreen.SetActive(false);
        BlackScreen.SetActive(true);
    }

    public void ShowWhiteScreen()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        WhiteScreen.SetActive(true);
        BlackScreen.SetActive(false);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAnim());
    }

    IEnumerator FadeOutAnim()
    {
        var t = 0f;
        canvasGroup.blocksRaycasts = true;
        while (t < FadeTime)
        {
            canvasGroup.alpha = 1f - (t / FadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInAnim());
    }

    IEnumerator FadeInAnim()
    {
        var t = 0f;
        canvasGroup.blocksRaycasts = true;
        while (t < FadeTime)
        {
            canvasGroup.alpha = t / FadeTime;
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
