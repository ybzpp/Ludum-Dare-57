using System.Collections;
using UnityEngine;

public class InteractButton : InteractableObject
{
    public Vector3 PressDepth = new Vector3(0f,0f, -0.005f); // Глубина вдавливания кнопки
    public float PressDuration = 0.1f; // Длительность анимации нажатия

    private Vector3 _initialPosition;
    private bool _isPressed = false;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Сохраняем начальную позицию кнопки
        _initialPosition = transform.localPosition;
    }

    public override void Use()
    {
        if (_isPressed) return; // Предотвращаем повторное нажатие во время анимации

        base.Use();
        StartCoroutine(AnimateButtonPress());
    }

    private IEnumerator AnimateButtonPress()
    {
        _isPressed = true;

        // 1. Вдавливаем кнопку
        Vector3 targetPosition = _initialPosition - PressDepth;
        float timer = 0;

        while (timer < PressDuration)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(_initialPosition, targetPosition, timer / PressDuration);
            yield return null;
        }

        transform.localPosition = targetPosition; // Убеждаемся, что кнопка точно в конечном положении

        // Пауза (можно убрать, если не нужна задержка)
        //yield return new WaitForSeconds(0.1f);

        // 2. Возвращаем кнопку в исходное положение
        timer = 0;
        while (timer < PressDuration)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(targetPosition, _initialPosition, timer / PressDuration);
            yield return null;
        }

        transform.localPosition = _initialPosition; // Убеждаемся, что кнопка точно в исходном положении
        _isPressed = false;
    }
}