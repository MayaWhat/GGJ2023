using System.Collections;
using UnityEngine;

public class ActivatedMover : Activatee
{
    private Vector2 _startingPosition;
    [SerializeField] private Vector2 _destinationPosition;
    [SerializeField] private float _movementSpeed;
    private Coroutine _currentMovement;

    private void Awake()
    {
        _startingPosition = transform.position;
    }

    public override void Activate()
    {
        if (_currentMovement != null)
        {
            StopCoroutine(_currentMovement);
        }

        _currentMovement = StartCoroutine(DoMove(_destinationPosition));
    }

    public override void Deactivate()
    {
        if (_currentMovement != null)
        {
            StopCoroutine(_currentMovement);
        }

        _currentMovement = StartCoroutine(DoMove(_startingPosition));
    }

    IEnumerator DoMove(Vector2 destination)
    {
        while ((Vector2)transform.position != destination)
        {
            transform.position = Vector2.Lerp(transform.position, destination, _movementSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}