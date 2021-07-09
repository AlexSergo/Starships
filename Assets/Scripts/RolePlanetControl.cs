using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RolePlanetControl : MonoBehaviour
{
    private static Planet[] _planets;
    private static UnityEvent OnRolePlanetChanged = new UnityEvent();

    public static void ChangePlanetsRole()
    {
        if (_planets == null)
            InitPlanets();
        OnRolePlanetChanged?.Invoke();
    }

    private static void InitPlanets()
    {
        _planets = FindObjectsOfType<Planet>();
        for (int i = 0; i < _planets.Length; i++)
            OnRolePlanetChanged.AddListener(_planets[i].ChangeManagedPlanet);
    }
}
