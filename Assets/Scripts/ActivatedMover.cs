using System.Collections;
using UnityEngine;

public class ActivatedMover : Activatee
{
    private Vector2 _startingPosition;
    [SerializeField] private Vector2 _destinationPosition;
    [SerializeField] private float _activateSpeed;
    [SerializeField] private float _deactivateSpeed;
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

        _currentMovement = StartCoroutine(DoMove(_destinationPosition, _activateSpeed));
    }

    public override void Deactivate()
    {
        if (_currentMovement != null)
        {
            StopCoroutine(_currentMovement);
        }

        _currentMovement = StartCoroutine(DoMove(_startingPosition, _deactivateSpeed));
    }

    IEnumerator DoMove(Vector2 destination, float speed)
    {
        while ((Vector2)transform.position != destination)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, destination, speed);
            yield return new WaitForFixedUpdate();
        }
    }
}