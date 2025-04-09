using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro; // Для использования List

public class Elevator : MonoBehaviour
{
    public string ID;

    public bool CanUse => !Broken && !NotEnergy;
    public bool Broken;
    public bool NotEnergy;
    

    [Tooltip("Массив дверей лифта")]
    public ElevatorDoor[] elevatorDoors;
    [Tooltip("Скорость движения лифта")]
    public float elevatorSpeed = 5f;
    [Tooltip("Время ожидания перед закрытием дверей (в секундах)")]
    public float doorCloseDelay = 2f;

    public Lamp Lamp;
    public TMP_Text NumberDisplayText;

    [Header("SFX")]
    public AudioClip CloseDoorAudio;
    public AudioClip OpenDoorAudio;
    public AudioSource DoorAudioSource;
    public AudioSource InElevatorAudioSource;

    private int currentFloor = 0;
    private bool isMoving = false;
    private bool doorsOpen = false;
    private Coroutine isReadyCoroutine;
    private Coroutine moveCoroutine; // Добавлено для контроля корутины движения
    private Queue<int> floorRequestQueue = new Queue<int>(); //Очередь запросов этажей

    private bool isReady;

    public List<ElevatorButton> elevatorButtons;

    private void Start()
    {
        if (elevatorDoors == null || elevatorDoors.Length == 0)
        {
            Debug.LogError("Не назначены двери для лифта!");
        }

        foreach (var button in elevatorButtons)
            button.SetElevatorKey(ID);

        isReady = true;
    }

    public void Fix()
    {
        Broken = false;
        UpdateState();
    }

    public void SetEnergy()
    {
        NotEnergy = false;
        Lamp.Enable();

        UpdateState();
    }
    

    public void EnergyDisable()
    {
        NotEnergy = true;
        Lamp.Disable();

        UpdateState();
    }

    private void UpdateState()
    {
    }

    public void SetFloorToButtons(int floor)
    {
        //Debug.Log($"Elevator SetFloorToButtons: {floor}");

        foreach (var button in elevatorButtons)
            button.FloorNumber = floor;
    }

    private void OpenDoors()
    {
        if (!CanUse) return;
        if (elevatorDoors == null) return;

        foreach (var e in elevatorDoors)
        {
            e.Open();
        }

        doorsOpen = true;
        DoorAudioSource.PlayOneShot(OpenDoorAudio);
        PadikService.Floors[currentFloor].OpenDoors(ID);

        StartIsReady();

        if (currentFloor == 3)
        {
            var e = PadikService.GetOtherElevator(ID);
            if (e != null)
            {
                e.Teleport(currentFloor);
            }
        } 
    }

    private void Teleport(int f)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = PadikService.Floors[f].TargetPoint.position.y;

        currentFloor = f;
        transform.position = targetPosition;

        Lamp.DisableFlicker();

        var floor = PadikService.Floors.Where(x => x.FloorNumber == currentFloor)?.First();
        if (floor != null)
        {
            if (floor.ElectricalPanel)
            {
                if (!floor.ElectricalPanel.IsEnabled)
                {
                    EnergyDisable();
                }
            }
        }

        float targetFloorNumber = PadikService.Floors[currentFloor].FloorDisplayNumber;
        NumberDisplayText.text = ((int)targetFloorNumber).ToString();
    }

    private void CloseDoors()
    {
        if (!CanUse) return;
        if (elevatorDoors == null) return;

        foreach (var e in elevatorDoors)
        {
            e.Close();
        }

        doorsOpen = false;
        DoorAudioSource.PlayOneShot(CloseDoorAudio);
        PadikService.Floors[currentFloor].CloseDoors(ID);
    }

    private void StartIsReady()
    {
        // Если корутина уже запущена, останавливаем ее
        if (isReadyCoroutine != null)
        {
            StopCoroutine(isReadyCoroutine);
        }
        isReadyCoroutine = StartCoroutine(IsReadyAfterDelay());
    }

    private IEnumerator IsReadyAfterDelay()
    {
        yield return new WaitForSeconds(doorCloseDelay);
        isReady = true;
    }


    public void GoToFloor(int floorNumber)
    {
        if (floorNumber >= 0 && floorNumber < PadikService.Floors.Length)
        {
            floorRequestQueue.Enqueue(floorNumber); // Добавляем запрос в очередь
            if (!isMoving)
            {
                ProcessFloorRequest(); // Запускаем обработку, если лифт не двигается
            }
        }
        else
        {
            Debug.LogError("Недопустимый номер этажа.");
        }
    }

    private void ProcessFloorRequest()
    {
        if (floorRequestQueue.Count > 0 && !isMoving)
        {
            int targetFloor = floorRequestQueue.Dequeue(); // Берем следующий этаж из очереди
            if (doorsOpen)
            {
                CloseDoors();
                StartCoroutine(DelayedMoveElevator(targetFloor));
            }
            else
            {
                StartCoroutine(MoveElevator(targetFloor));
            }
        }
    }


    private IEnumerator MoveElevator(int targetFloor)
    {
        isMoving = true;
        InElevatorAudioSource.Play();
        Lamp.EnableFlicker();

        if (doorsOpen) CloseDoors(); //Close doors if they are open

        while (doorsOpen)
            yield return null;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position;
        targetPosition.y = PadikService.Floors[targetFloor].TargetPoint.position.y;

        float startFloorNumber = PadikService.Floors[currentFloor].FloorDisplayNumber;
        float targetFloorNumber = PadikService.Floors[targetFloor].FloorDisplayNumber;

        float totalDistance = Mathf.Abs(targetPosition.y - startPosition.y);
        float remainingDistance = totalDistance;

        float distance = Vector3.Distance(transform.position, targetPosition);
        while (distance > elevatorSpeed * Time.deltaTime * 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, elevatorSpeed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, targetPosition);

            remainingDistance = Mathf.Abs(transform.position.y - targetPosition.y);

            // Правильный расчет normalizeValue (0-1) на основе пройденного пути
            float normalizeValue = 1f - (remainingDistance / totalDistance);
            NumberDisplayText.text = ((int)Mathf.Lerp(startFloorNumber, targetFloorNumber, normalizeValue)).ToString();
            yield return null;
        }

        NumberDisplayText.text = ((int)targetFloorNumber).ToString();
        transform.position = targetPosition;
        currentFloor = targetFloor;
        isMoving = false;
        InElevatorAudioSource.Stop();
        OpenDoors();
        ProcessFloorRequest(); // Обрабатываем следующий запрос из очереди

        Lamp.DisableFlicker();

        var floor = PadikService.Floors.Where(x => x.FloorNumber == currentFloor)?.First();
        if (floor != null)
        {
            if (floor.ElectricalPanel)
            {
                if (!floor.ElectricalPanel.IsEnabled)
                {
                    EnergyDisable();
                }
            }
        }
    }

    //Helper method to add a small delay before calling elevator
    private IEnumerator DelayedMoveElevator(int targetFloor)
    {
        while (doorsOpen)
            yield return null; //wait till doors are closed
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveElevator(targetFloor));
    }


    // Добавленный метод Call
    public void Call(int floorNumber)
    {
        //Debug.Log($"Elevator Call key:{ID} floorNumber:{floorNumber} CanUse:{CanUse} isMoving:{isMoving} isReady:{isReady} ");

        if (!CanUse)
            return;

        if (isMoving || !isReady)
        {
            return;
        }

        isReady = false;

        if (floorNumber >= 0 && floorNumber < PadikService.Floors.Length)
        {
            if (currentFloor == floorNumber)
            {
                if (!doorsOpen)
                {
                    OpenDoors();
                }
                else
                {
                    Debug.Log("Двери уже открыты.");
                }
            }
            else
            {
                // Если лифт не на нужном этаже, отправляем его туда
                GoToFloor(floorNumber);
            }
        }
        else
        {
            Debug.LogError("Недопустимый номер этажа для вызова.");
        }
    }
}