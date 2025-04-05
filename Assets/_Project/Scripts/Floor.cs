using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ElevatorDoorsGroup
{
    public string Key;
    public ElevatorDoor[] Doors;
}
    

public class Floor : MonoBehaviour
{
    public int FloorNumber;
    public Transform TargetPoint;
    public GameObject[] FloorNumbers;
    public ElevatorButton[] Buttons;
    public ElevatorDoorsGroup[] Doors;

    private void Start()
    {
        foreach (var button in Buttons)
        {
            button.FloorNumber = FloorNumber;
        }

        for (var i = 0; i < FloorNumbers.Length; i++) 
        {
            FloorNumbers[i].SetActive(i == FloorNumber); 
        }
    }

    public void OpenDoors(string key)
    {
        Debug.Log($"FloorNumber:{FloorNumber} OpenDoors key:{key}");


        Doors.Where(x => x.Key == key).ToList().ForEach(x =>
        {
            foreach (var item in x.Doors)
            {
                item.Open();
            }
        });
    }

    public void CloseDoors(string key)
    {
        Doors.Where(x => x.Key == key).ToList().ForEach(x =>
        {
            foreach (var item in x.Doors)
            {
                item.Close();
            }
        });
    }
}
