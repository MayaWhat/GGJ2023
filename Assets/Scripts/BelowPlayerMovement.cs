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
    private BelowPlayerSounds _playerSounds;
    private Vector2 _moveDirection;
    private int _timer;
    private List<GameObject> _rootTrail;
    private bool _isRetracting = false;
    private Vector3? _retractEnd = null;
    private bool _retractForwards = false;
    private bool _atSurface;
    private int _tilemapMask;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _playerSounds = GetComponent<BelowPlayerSounds>();
        _tilemapMask = LayerMask.GetMask("Tilemap");
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
        ServiceLocator.Instance.MusicController.Dark();
        _rootTrail = new List<GameObject>();
        _isRetracting = false;

        _playerSounds.Burrow.Play();
        _playerSounds.Digging.Play();
        _renderer.enabled = true;
        _retractEnd = null;
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
                StartRetract(false);
                return;
            }
        }

        if (_atSurface && _moveDirection.y == 1)
        {
            _renderer.enabled = false;
            _retractEnd = new Vector3(transform.position.x, transform.position.y + 1);
            StartRetract(true);
        }

        var aboveHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1), new Vector2(0, 1), 1f, _tilemapMask);
        _atSurface = aboveHit.collider == null;
    }

    private void StartRetract(bool forwards = false)
    {
        _retractForwards = forwards;
        _moveTimer = 5;
        _isRetracting = true;
        _playerControls.Below.Disable();
    }

    private void Retract()
    {
        _renderer.enabled = false;

        if (_rootTrail.Count == 0)
        {
            ServiceLocator.Instance.PlayerManager.Switch(_retractEnd, toAbove: true);
            return;
        }

        GameObject root;
        if (_retractForwards)
        {
            root = _rootTrail.First();
        }
        else
        {
            root = _rootTrail.Last();
        }

        root.SetActive(false);
        _rootTrail.Remove(root);
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