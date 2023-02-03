using UnityEngine;
using UnityEngine.InputSystem;

public class BelowPlayerMovement : MonoBehaviour, PlayerControls.IBelowActions
{
    [SerializeField] private PlayerManager _playerManager;
    private PlayerControls _playerControls;
    [SerializeField] private GameObject _belowTrail;
    [SerializeField] private int _moveTimer = 50;
    private Vector2 _moveDirection;
    private int _timer;

    public void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Below.SetCallbacks(this);
        }
        _playerControls.Below.Enable();
        _moveDirection = new Vector2(0, -1);
        _timer = _moveTimer;
    }

    private void OnDisable()
    {
        if (_playerControls != null)
        {
            _playerControls.Below.Disable();
        }
    }

    private void FixedUpdate()
    {
        if (_timer == 0)
        {
            Move();
            _timer = _moveTimer;
        }
        else
        {
            _timer--;
        }
    }

    private void Move()
    {
        GameObject trail = Instantiate(_belowTrail, transform.position, transform.rotation);
        transform.position = new Vector3(transform.position.x + _moveDirection.x, transform.position.y + _moveDirection.y);

        if (transform.position.y >= 0)
        {
            _playerManager.Switch(toAbove: true);
        }
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