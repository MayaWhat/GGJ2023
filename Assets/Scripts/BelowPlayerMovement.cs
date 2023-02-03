using UnityEngine;
using UnityEngine.InputSystem;

public class BelowPlayerMovement : MonoBehaviour, PlayerControls.IBelowActions
{
    private PlayerControls _playerControls;
    [SerializeField] private GameObject _belowTrail;
    [SerializeField] private int _moveTimer;
    private Vector2 _moveDirection;

    private BelowPlayerSounds _playerSounds;

    private void Awake()
    {
        _playerSounds = GetComponent<BelowPlayerSounds>();
    }

    public void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Below.SetCallbacks(this);
        }
        _playerControls.Below.Enable();
        _moveDirection = new Vector2(0, -1);
        _playerSounds.Digging.Play();
    }

    private void OnDisable()
    {
        if (_playerControls != null)
        {
            _playerControls.Below.Disable();
        }

        _playerSounds.Digging.Stop();
    }

    private void FixedUpdate()
    {
        if (_moveTimer == 0)
        {
            Move();
            _moveTimer = 50;
        }
        else
        {
            _moveTimer--;
        }
    }

    private void Move()
    {
        GameObject trail = Instantiate(_belowTrail, transform.position, transform.rotation);
        transform.position = new Vector3(transform.position.x + _moveDirection.x, transform.position.y + _moveDirection.y);
    }

    #region MoveInputCallbacks

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (!context.performed || _moveDirection.y != 0)
        {
            return;
        }

        _moveDirection = new Vector2(0, 1);
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (!context.performed || _moveDirection.y != 0)
        {
            return;
        }

        _moveDirection = new Vector2(0, -1);
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!context.performed || _moveDirection.x != 0)
        {
            return;
        }

        _moveDirection = new Vector2(-1, 0);
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!context.performed || _moveDirection.x != 0)
        {
            return;
        }

        _moveDirection = new Vector2(1, 0);
    }

    #endregion MoveInputCallbacks
}