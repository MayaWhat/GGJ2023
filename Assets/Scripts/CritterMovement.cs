using UnityEngine;

public class CritterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRange = 5;
    [SerializeField] private float _jumpHeight = 5;
    private float _xStartPosition;
    private bool _movingLeft = false;
    [SerializeField] private MoveType _moveType;
    private Rigidbody2D _rigidBody;
    private AnimationManager _animationManager;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _xStartPosition = transform.position.x;
        if (_moveType == MoveType.Random && Random.Range(0, 1) == 1)
        {
            _movingLeft = true;
        }
        _animationManager = GetComponent<AnimationManager>();
    }

    private void FixedUpdate()
    {
        if (_moveType == MoveType.Idle)
        {
            return;
        }

        if (_moveType == MoveType.Patrol)
        {
            if (Mathf.Abs(transform.position.x - _xStartPosition) > _moveRange)
            {
                if (transform.position.x > _xStartPosition) { _movingLeft = true; } else { _movingLeft = false; }
            }
        }

        if (_moveType == MoveType.Random || _moveType == MoveType.RandomJumping)
        {
            if (Mathf.Abs(transform.position.x - _xStartPosition) > _moveRange || Random.Range(0, 128) == 0)
            {
                if (transform.position.x > _xStartPosition) { _movingLeft = true; } else { _movingLeft = false; }
            }

            if (Random.Range(0, 64) == 0)
            {
                _animationManager.SetWalking(true);
                return;
            }
        }

        var horizontalVelocity = (_movingLeft ? -1f : 1f) * _moveSpeed;

        _rigidBody.velocity = new Vector2(horizontalVelocity, _rigidBody.velocity.y);

        _animationManager.SetWalking(true);

        if (!_movingLeft)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (_moveType == MoveType.RandomJumping && Random.Range(0, 512) == 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpHeight);
        }
    }
}

public enum MoveType
{
    Idle,
    Patrol,
    Random,
    RandomJumping
}