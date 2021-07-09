using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    private const string  PATH_TO_PLANET = "Prefabs/Planet";
    private const int     WIDTH = 8; 
    private const int     HEIGHT = 4; 
    private GameObject    _planet;
    private System.Random _random = new System.Random();
    public int            CountOfPlanets;

    void Start()
    {
        CountOfPlanets = CountOfPlanets > 10 ? 10 : CountOfPlanets;
        CountOfPlanets = CountOfPlanets == 0 ? _random.Next(1, 11) : CountOfPlanets;
        CreatePlanets();
    }

    private void CreatePlanets()
    {
        var planets = new Dictionary<Vector2, float>();
        bool hasCapturedPlanet = false;
        for (int i = 0; i < CountOfPlanets; i++)
        {
            float planetScale = (float)(_random.NextDouble() * (2 - 0.5f) + 0.5f);
            Vector2 position = GetRandomPlanetPosition();
            while (!IsPositionGood(position, planetScale / 2, planets))
                position = GetRandomPlanetPosition();

            GameObject planet = Instantiate((GameObject)Resources.Load(PATH_TO_PLANET), position, Quaternion.identity);
            planets.Add(planet.transform.position, planetScale / 2);
            if (!hasCapturedPlanet)
            {
                hasCapturedPlanet = true;
                planet.GetComponent<Planet>().SetCaptured(hasCapturedPlanet);
            }
            planet.transform.localScale = new Vector3(planetScale, planetScale);
        }
    }

    private bool IsPositionGood(Vector2 position, float radius, Dictionary<Vector2, float> planets)
    {
        foreach (var pair in planets)
            if (Vector2.Distance(position, pair.Key) < radius + pair.Value)
                return false;
        return true;
    }

    private Vector2 GetRandomPlanetPosition()
    {
        return new Vector2((float)(-WIDTH + _random.NextDouble() * WIDTH * 2),
                                        (float)(-HEIGHT + _random.NextDouble() * HEIGHT * 2));
    }
}
