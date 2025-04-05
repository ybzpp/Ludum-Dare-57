using UnityEngine;
using System.Collections;
using System.Collections.Generic; // ��� ������������� List

public class Elevator : MonoBehaviour
{
    public string ID;

    [Tooltip("������ ������ �����")]
    public ElevatorDoor[] elevatorDoors;
    [Tooltip("�������� �������� �����")]
    public float elevatorSpeed = 5f;
    [Tooltip("����� �������� ����� ��������� ������ (� ��������)")]
    public float doorCloseDelay = 2f;

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

    private void Start()
    {
        isReady = true;

        if (elevatorDoors == null || elevatorDoors.Length == 0)
        {
            Debug.LogError("�� ��������� ����� ��� �����!");
        }
    }

    private void OpenDoors()
    {
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