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
    public int FloorDisplayNumber;

    public Transform TargetPoint;
    public FloorButton[] Buttons;
    public ElevatorDoorsGroup[] Doors;
    public ElectricalPanel ElectricalPanel;

    public List<Renderer> Renderers;
    public List<Lamp> Lamps;

    private void Start()
    {
        foreach (var button in Buttons)
        {
            button.SetFloorNumber(FloorNumber);
        }

        ElectricalPanel.OnEnablePanel += EnergyEnable;
        ElectricalPanel.OnDisablePanel += EnergyDisable;

        EnergyDisable();
    }

    private void OnDestroy()
    {
        ElectricalPanel.OnEnablePanel -= EnergyEnable;
        ElectricalPanel.OnDisablePanel -= EnergyDisable;
    }

    private void EnergyDisable()
    {
        Debug.Log($"Floor:{FloorNumber} EnergyDisable");

        PadikService.DisableElevators();

        foreach (var l in Lamps)
            l.Disable();
    }

    private void EnergyEnable()
    {
        Debug.Log($"Floor:{FloorNumber} EnergyEnable");

        PadikService.EnableElevators();

        foreach (var l in Lamps)
        {
            l.Enable();
            l.DisableFlicker();
        }

        foreach (var e in PadikService.Elevators)
        {
            e.SetFloorToButtons(FloorNumber + 1);
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
