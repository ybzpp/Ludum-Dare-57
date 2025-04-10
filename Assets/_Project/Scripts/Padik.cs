﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PadikService
{
    public static Floor[] Floors;
    public static List<Elevator> Elevators;
    public static void CallElevator(string key, int number)
    {
        //Debug.Log($"CallElevator key:{key} number:{number} ");

        var es = Elevators.Where(x => x.ID == key);
        foreach (var e in es)
            e.Call(number);
    }

    internal static void DisableElevators()
    {
        foreach (var e in Elevators)
            e.EnergyDisable();
    }

    internal static void EnableElevators() 
    {
        foreach (var e in Elevators)
            e.SetEnergy();
    }

    internal static Elevator GetOtherElevator(string iD)
    {
        return Elevators.Where(x => x.ID != iD).First();
    }
}

public class Padik : MonoBehaviour
{
    public Transform StartPoint;
    public Elevator[] Elevator;
    public Floor[] Floors;

    private void Awake()
    {
        PadikService.Elevators = Elevator.ToList();
        PadikService.Floors = Floors;
    }
}

[System.Serializable]
public enum SurfaceType
{
    None,
    Wood,
    Metal,
    Rock,
    Tile,
    Snow
}
