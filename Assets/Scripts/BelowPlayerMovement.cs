using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelowPlayerMovement : MonoBehaviour, PlayerControls.IBelowActions
{
    private PlayerControls _playerControls;
    [SerializeField] private GameObject _belowTrail;
    [SerializeField] private int _moveTimer = 30;
    private SpriteRenderer _renderer;
    private Vector2 _moveDirection;
    private int _timer;
    private List<GameObject> _rootTrail;
    private bool _isRetracting = false;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
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
        _moveTimer = 30;
        _timer = _moveTimer;
        ServiceLocator.Instance.Camera.Follow = transform;
        _rootTrail = new List<GameObject>();
        _isRetracting = false;
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
            if (_isRetracting)
            {
                Retract();
            }
            else
            {
                Move();
            }

            _timer = _moveTimer;
        }
        else
        {
            _timer--;
        }
    }

    private void Move()
    {
        GameObject root = Instantiate(_belowTrail, transform.position, transform.rotation);
        _rootTrail.Add(root);
        transform.position = new Vector3(transform.position.x + _moveDirection.x, transform.position.y + _moveDirection.y);

        foreach (var trail in _rootTrail)
        {
            if (trail.transform.position == transform.position)
            {
                StartRetract();
                return;
            }
        }

        if (transform.position.y >= 0)
        {
            ServiceLocator.Instance.PlayerManager.Switch(new Vector3(transform.position.x, 1), toAbove: true);
        }
    }

    private void StartRetract()
    {
        _moveTimer = 5;
        _isRetracting = true;
        _playerControls.Below.Disable();
    }

    private void Retract()
    {
        _renderer.enabled = false;

        if (_rootTrail.Count == 0)
        {
            ServiceLocator.Instance.PlayerManager.Switch(toAbove: true);
            return;
        }

        var last = _rootTrail.Last();
        last.SetActive(false);
        _rootTrail.Remove(last);
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