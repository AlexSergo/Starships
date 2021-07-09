using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCapture : MonoBehaviour
{
    private Planet[] _planets;

    public void Capture(Planet planet)
    {
        var managedPlanet = FindManegedPlanet();
        if (managedPlanet != null)
         managedPlanet.SendStarships(planet.transform.position);
    }

    private Planet FindManegedPlanet()
    {
        if (_planets == null)
             _planets = FindObjectsOfType<Planet>();
        for (int i = 0; i < _planets.Length; i++)
            if (_planets[i].IsManagedPlanet)
                return _planets[i];
        return null;
    }
}
