using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public bool            IsCaptured = false;
    public bool            IsManagedPlanet {get; private set;} = false;
    private const string   PATH_TO_STARSHIP = "Prefabs/Starship";
    private TextMeshPro    _textMesh;
    private int            StarshipCount;
    private PlanetCapture  _planetCapture;
    private Transform      _transform;

    private SpriteRenderer _spriteRenderer;
    private float          _timer = 5f;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = _transform.GetComponent<SpriteRenderer>();
        _textMesh = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        _planetCapture = FindObjectOfType<PlanetCapture>();

        StarshipCount = IsCaptured ? 50 : UnityEngine.Random.Range(10,30);
        _textMesh.text = StarshipCount.ToString();
    }

    private Starship CreateStarship()
    {
        if (IsCaptured && StarshipCount > 1)
        {
            Vector3 position = CirclePosition(_transform.position , _transform.localScale.x / 2);
            var starship = (GameObject)Instantiate(Resources.Load(PATH_TO_STARSHIP),position, Quaternion.identity);
            return starship.GetComponent<Starship>();
        }
        return null;
    }

    private Vector2 CirclePosition(Vector2 center, float radius)
    {
        float radiusIncrease = 0.3f;
        Vector2 pos;
        float maximum = center.x + radius + radiusIncrease;
        float minimum = center.x - radius - radiusIncrease;
        pos.x = (float)( UnityEngine.Random.Range(minimum, maximum));
        var positiveOrNegative = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1; 
        pos.y = (float)(positiveOrNegative * Math.Sqrt(Math.Pow(radius + radiusIncrease, 2) - Math.Pow(pos.x - center.x,2)) + center.y);
         return pos;
    }

    public void SetCaptured(bool isCaptured)
    {
        IsCaptured = isCaptured;
        if (isCaptured)
            _spriteRenderer.color = Color.blue;
    }

    private void OnMouseDown()
    {
        if (IsManagedPlanet)
        {
            SetManagedStatus(false);
            return;
        }
        if (IsCaptured)
        {
            RolePlanetControl.ChangePlanetsRole();
            SetManagedStatus(true);
        }
        else
            _planetCapture.Capture(this);
    }

    public void ChangeManagedPlanet()
    {
        SetManagedStatus(false);
    }

    private void SetManagedStatus(bool status)
    {
        IsManagedPlanet = status;
        if (IsCaptured)
            if (!status)
                _spriteRenderer.color = Color.blue;
            else
                _spriteRenderer.color = new Color(0,135,255);
    }

    public void SendStarships(Vector2 position)
    {
        int starshipsCountForFight = StarshipCount / 2;
        StartCoroutine(SendStarships(position, starshipsCountForFight));
    }

    private IEnumerator SendStarships(Vector2 position, int starshipsCount)
    {
        for (int i = 0; i < starshipsCount; i++)
        {
            CreateStarship()?.FlyToPlanet(position);
            ChangeStarshipCount(-1);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void ChangeStarshipCount(int increase)
    {
        StarshipCount += increase;
        _textMesh.text = StarshipCount.ToString();
    }

    public void GetDamage()
    {
        if (!IsCaptured)
        {
            if (StarshipCount > 0)
                ChangeStarshipCount(-1);
            CheckCapture();
        }
    }

    private void CheckCapture()
    {
        if (StarshipCount == 0)
        {
            IsCaptured = true;
            _spriteRenderer.color = Color.blue;
        }
    }

    private void Update()
    {
        if (IsCaptured)
            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
            {
               ChangeStarshipCount(1);
              _timer = 5;
            }
    }
}
