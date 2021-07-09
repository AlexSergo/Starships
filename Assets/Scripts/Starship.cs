using System;
using UnityEngine;

public class Starship : MonoBehaviour
{
    private Vector3   _targetPosition;
    private bool      _isFly;
    private float     tick;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _isFly = false;
    }

    private void Update()
    {
        if (!_isFly) return;
        MoveToTarget();
        RotateToTarget();
    }

    private void MoveToTarget()
    {
        _transform.position = Vector2.Lerp(_transform.position, _targetPosition, tick);
        tick += 0.0001f;
    }

    private void RotateToTarget()
    {
        Vector3 direction = _targetPosition - transform.position;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position == _targetPosition)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Planet>().GetDamage();
        }
    }

    public void FlyToPlanet(Vector3 position)
    {
        _targetPosition = position;
       _isFly = true;
    }
}
