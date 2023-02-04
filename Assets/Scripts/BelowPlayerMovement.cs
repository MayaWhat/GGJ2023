using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelowPlayerMovement : MonoBehaviour, PlayerControls.IBelowActions
{
    private PlayerControls _playerControls;
    [SerializeField] private LineRenderer _belowTrail;
    [SerializeField] private Collider2D _belowTrailCollider;
    [SerializeField] private int _moveTimer = 10;
    private SpriteRenderer _renderer;
    private BelowPlayerSounds _playerSounds;
    private Vector2 _moveDirection;
    private int _timer;
    private LineRenderer _trail;
    private List<Collider2D> _trailColliders;
    private bool _isRetracting = false;
    private bool _retractForwards = false;
    private int _groundMask;
    private int _blockerMask;
    private int _critterMask;
    private GameObject _targetCritter;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _playerSounds = GetComponent<BelowPlayerSounds>();
        _groundMask = LayerMask.GetMask("Ground");
        _blockerMask = LayerMask.GetMask("HardGround");
        _critterMask = LayerMask.GetMask("Critters");
        _trail = Instantiate(_belowTrail);
        _trail.transform.position += new Vector3(0.5f, -0.5f, 0f);
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
        _renderer.transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        _moveTimer = 12;
        _timer = 0;
        ServiceLocator.Instance.Camera.Follow = transform;
        ServiceLocator.Instance.MusicController.Dark();
        _trailColliders = new List<Collider2D>();
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
        if (_trail.positionCount == 0)
        {
            _trail.positionCount += 2;
            _trail.SetPosition(_trail.positionCount - 2, transform.position + new Vector3(0f, 1f));
            _trail.SetPosition(_trail.positionCount - 1, transform.position);
        }

        // Check just the middle of the square rather than the full square to avoid detecting edges of adjacent tiles
        var isBlocker = Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _blockerMask);
        if (isBlocker.collider != null)
        {
            StartCoroutine(FlashRed());
            _playerSounds.Error.Play();
            StartRetract(false);
            return;
        }

        var isGround = Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _groundMask);
        if (isGround.collider != null)
        {
            StartCoroutine(DoMove());
            return;
        }

        var isCritter = Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.5f, 0f), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, _critterMask);
        if (isCritter.collider != null)
        {
            _targetCritter = isCritter.collider.gameObject;
            if (_targetCritter != null)
            {
                _playerSounds.Unburrow.Play();
                _renderer.enabled = false;
                StartRetract(true);
                return;
            }
        }

        StartCoroutine(FlashRed());
        _playerSounds.Error.Play();
        StartRetract(false);
    }

    private IEnumerator DoMove()
    {
        _trail.positionCount += 1;
        _trail.SetPosition(_trail.positionCount - 1, transform.position);
        var collider = Instantiate(_belowTrailCollider);
        collider.transform.position = transform.position;
        _trailColliders.Add(collider);
        var startingPosition = transform.position;
        var direction = _moveDirection;
        var moveAnimationTimer = _moveTimer / 2;

        if (direction == Vector2.up)
        {
            _renderer.transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
        }
        else if (direction == Vector2.right)
        {
            _renderer.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
        }
        else if (direction == Vector2.left)
        {
            _renderer.transform.rotation = Quaternion.AngleAxis(-90f, Vector3.forward);
        }
        else
        {
            _renderer.transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }

        for (var i = 1; i <= moveAnimationTimer; i++)
        {
            transform.position = Vector3.Lerp(startingPosition, startingPosition + (Vector3)direction, (float)i / moveAnimationTimer);
            _trail.SetPosition(_trail.positionCount - 1, transform.position);
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
        if (_trail.positionCount == 0)
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

        Collider2D collider = null;
        if (_retractForwards)
        {
            var positions = new Vector3[_trail.positionCount];
            _trail.GetPositions(positions);
            _trail.SetPositions(positions.Skip(1).ToArray());
            collider = _trailColliders.FirstOrDefault();
        }
        else
        {
            collider = _trailColliders.LastOrDefault();
            if (collider != null)
            {
                transform.position = collider.transform.position;
            }
            else
            {
                _renderer.enabled = false;
            }
        }

        if (collider != null)
        {
            Destroy(collider.gameObject);
            _trailColliders.Remove(collider);
        }

        _trail.positionCount -= 1;
        _moveTimer = _trail.positionCount > 0 ? 5 / _trail.positionCount : 5;
    }

    private IEnumerator FlashRed()
    {
        var time = 1f;
        var color = 0f;
        var direction = true;

        while (time > 0)
        {
            color += Time.deltaTime * 10 * (direction ? 1 : -1);
            if (color > 1f || color < 0f)
            {
                direction = !direction;
            }

            _renderer.color = Color.Lerp(Color.white, Color.red, color);

            yield return null;
            time -= Time.deltaTime;
        }

        _renderer.color = Color.white;
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