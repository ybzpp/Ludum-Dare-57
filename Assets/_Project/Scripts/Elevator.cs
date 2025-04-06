using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; // ��� ������������� List

public class Elevator : MonoBehaviour
{
    public string ID;

    public bool CanUse => !Broken && !NotEnergy;
    public bool Broken;
    public bool NotEnergy;
    

    [Tooltip("������ ������ �����")]
    public ElevatorDoor[] elevatorDoors;
    [Tooltip("�������� �������� �����")]
    public float elevatorSpeed = 5f;
    [Tooltip("����� �������� ����� ��������� ������ (� ��������)")]
    public float doorCloseDelay = 2f;

    public Lamp Lamp;

    [Header("SFX")]
    public AudioClip CloseDoorAudio;
    public AudioClip OpenDoorAudio;
    public AudioSource DoorAudioSource;
    public AudioSource InElevatorAudioSource;

    private int currentFloor = 0;
    private bool isMoving = false;
    private bool doorsOpen = false;
    private Coroutine isReadyCoroutine;
    private Coroutine moveCoroutine; // ��������� ��� �������� �������� ��������
    private Queue<int> floorRequestQueue = new Queue<int>(); //������� �������� ������

    private bool isReady;

    public List<ElevatorButton> elevatorButtons;

    private void Start()
    {
        if (elevatorDoors == null || elevatorDoors.Length == 0)
        {
            Debug.LogError("�� ��������� ����� ��� �����!");
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
        // ���� �������� ��� ��������, ������������� ��
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
            floorRequestQueue.Enqueue(floorNumber); // ��������� ������ � �������
            if (!isMoving)
            {
                ProcessFloorRequest(); // ��������� ���������, ���� ���� �� ���������
            }
        }
        else
        {
            Debug.LogError("������������ ����� �����.");
        }
    }

    private void ProcessFloorRequest()
    {
        if (floorRequestQueue.Count > 0 && !isMoving)
        {
            int targetFloor = floorRequestQueue.Dequeue(); // ����� ��������� ���� �� �������
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

        Vector3 targetPosition = transform.position;
        targetPosition.y = PadikService.Floors[targetFloor].TargetPoint.position.y;

        float distance = Vector3.Distance(transform.position, targetPosition);

        while (distance > elevatorSpeed * Time.deltaTime * 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, elevatorSpeed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, targetPosition);
            yield return null;
        }

        transform.position = targetPosition;
        currentFloor = targetFloor;
        isMoving = false;
        InElevatorAudioSource.Stop();
        OpenDoors();
        ProcessFloorRequest(); // ������������ ��������� ������ �� �������

        Lamp.DisableFlicker();
        EnergyDisable();
    }

    //Helper method to add a small delay before calling elevator
    private IEnumerator DelayedMoveElevator(int targetFloor)
    {
        while (doorsOpen)
            yield return null; //wait till doors are closed
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveElevator(targetFloor));
    }


    // ����������� ����� Call
    public void Call(int floorNumber)
    {
        Debug.Log($"Elevator Call key:{ID} floorNumber:{floorNumber} CanUse:{CanUse} isMoving:{isMoving} isReady:{isReady} ");

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
                    Debug.Log("����� ��� �������.");
                }
            }
            else
            {
                // ���� ���� �� �� ������ �����, ���������� ��� ����
                GoToFloor(floorNumber);
            }
        }
        else
        {
            Debug.LogError("������������ ����� ����� ��� ������.");
        }
    }
}