using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField] private Vector3 closePosition;
    [SerializeField] private Vector3 openPosition;
    [Tooltip("Длительность анимации открытия/закрытия двери в секундах.")]
    [SerializeField] private float animationDuration = 1f;
    [Tooltip("Кривая анимации для более плавного движения.")]
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1); // Линейная кривая по умолчанию

    private bool isOpen = false;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        transform.localPosition = closePosition;
    }

    public void Open()
    {
        if (!isOpen)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveDoor(openPosition));
            isOpen = true;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveDoor(closePosition));
            isOpen = false;
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.localPosition;
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            float timeRatio = timeElapsed / animationDuration;  // Процент завершения анимации (0-1)
            float curveValue = movementCurve.Evaluate(timeRatio); // Получаем значение из AnimationCurve

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
    }
}