using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelowPlayerMovement : MonoBehaviour, PlayerControls.IBelowActions
{
    private PlayerControls _playerControls;
    [SerializeField] private GameObject _belowTrail;
    [SerializeField] private int _moveTimer = 20;
    private SpriteRenderer _renderer;
    private BelowPlayerSounds _playerSounds;
    private Vector2 _moveDirection;
    private int _timer;
    private List<GameObject> _rootTrail;
    private bool _isRetracting = false;
    private bool _retractForwards = false;
    private int _groundMask;
    private int _blockerMask;
    private int _critterMask;
    private GameObject _targetCritter;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _playerSounds = GetComponent<BelowPlayerSounds>();
        _groundMask = LayerMask.GetMask("Ground");
        _blockerMask = LayerMask.GetMask("HardGround");
        _critterMask = LayerMask.GetMask("Critters");
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
        _moveTimer = 20;
        _timer = 0;
        ServiceLocator.Instance.Camera.Follow = transform;
        ServiceLocator.Instance.MusicController.Dark();
        _rootTrail = new List<GameObject>();
        _isRetracting = false;

        _playerSounds.Burrow.Play();
        _playerSounds.Digging.Play();
        _renderer.enabled = true;
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
        // Check just the middle of the square rather than the full square to avoid detecting edges of adjacent tiles
        var isBlocker = Physics2D.BoxCast(transform.position + (Vector3)_moveDirection + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _blockerMask);
        if (isBlocker.collider == null)
        {
            var root = Instantiate(_belowTrail, transform.position, transform.rotation);
            _rootTrail.Add(root);

            var isGround = Physics2D.BoxCast(transform.position + (Vector3)_moveDirection + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _groundMask);
            if (isGround.collider != null)
            {
                StartCoroutine(DoMove());
                return;
            }

            var isCritter = Physics2D.BoxCast(transform.position + (Vector3)_moveDirection + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _critterMask);
            if (isCritter.collider != null)
            {
                _targetCritter = isCritter.collider.gameObject;
                if (_targetCritter != null)
                {
                    StartRetract(true);
                    return;
                }
            }
        }

        StartRetract(false);
    }

    private IEnumerator DoMove()
    {
        var startingPosition = transform.position;
        var direction = _moveDirection;

        for (var i = 1; i <= _moveTimer; i++)
        {
            transform.position = startingPosition + (Vector3)(direction * ((float)i / _moveTimer));
            yield return new WaitForFixedUpdate();
        }
    }

    private void StartRetract(bool forwards = false)
    {
        _retractForwards = forwards;
        _moveTimer = 2;
        _isRetracting = true;
        _playerControls.Below.Disable();
    }

    private void Retract()
    {
        _renderer.enabled = false;

        if (_rootTrail.Count == 0)
        {
            if (_targetCritter != null)
            {
                ServiceLocator.Instance.PlayerManager.Possess(_targetCritter);
            }
            else
            {
                ServiceLocator.Instance.PlayerManager.Return();
            }
            
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
        _moveTimer = _rootTrail.Count > 0 ? 8 / _rootTrail.Count : 8; 
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