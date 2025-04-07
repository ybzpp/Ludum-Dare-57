using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private string fullText = "Hello World!";
    [SerializeField] private float typingSpeed = 0.1f; // ����� ����� �������
    [SerializeField] private float displayDuration = 2f; // ����� ����������� ������� ������
    [SerializeField] private float fadeOutDuration = 1f; // ����� ������������
    private Coroutine typingCoroutine;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] int stepSfx = 3;
    int letterCount;


    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TMP_Text>();
            fullText = textComponent.text;
            textComponent.text = "";
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        // ������� ����� ����� �������
        textComponent.text = "";
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f);

        // ���������� ������� �����
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;

            letterCount++;
            if (letterCount % stepSfx == 0)
                AudioHelper.PlaySound("TextTyper", audioSource);


            yield return new WaitForSeconds(typingSpeed);
        }

        // ��� ��������� �����
        yield return new WaitForSeconds(displayDuration);

        // ������ ��������
        if (fadeOutDuration > 0)
        {
            float fadeTime = 0;
            Color startColor = textComponent.color;

            while (fadeTime < fadeOutDuration)
            {
                fadeTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeOutDuration);
                textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        // ��������� �������� ����� � �����
        textComponent.text = "";
    }

    // ��� ������ �� ������ �������� � ������ �����������
    public void ShowNewText(string newText, float newTypingSpeed, float newDisplayDuration, float newFadeOutDuration)
    {
        fullText = newText;
        typingSpeed = newTypingSpeed;
        displayDuration = newDisplayDuration;
        fadeOutDuration = newFadeOutDuration;

        StartTyping();
    }
}