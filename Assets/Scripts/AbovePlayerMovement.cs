using UnityEngine;
using UnityEngine.InputSystem;

public class AbovePlayerMovement : MonoBehaviour, PlayerControls.IAboveActions
{
    private PlayerControls _playerControls;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 30f;
    private Rigidbody2D _rigidBody;
    private AbovePlayerSounds _playerSounds;
    private int _groundMask;
    private int _hardGroundMask;
    private bool _onGround;
    private bool _onHardGround;
    private AnimationManager _animationManager;
    [HideInInspector]public bool IsRooted = false;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] public bool IsGoal = false;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerSounds = GetComponent<AbovePlayerSounds>();
        _groundMask = LayerMask.GetMask("Ground");
        _hardGroundMask = LayerMask.GetMask("HardGround");
        _animationManager = GetComponentInChildren<AnimationManager>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (IsGoal)
        {
            _spriteRenderer.color = new Color(1f, 1f, 0.8f);
        }
    }

    public void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Above.SetCallbacks(this);
        }
        _playerControls.Above.Enable();
        ServiceLocator.Instance.Camera.Follow = transform;
        ServiceLocator.Instance.MusicController.Happy();
        _spriteRenderer.color = new Color(1f, 0.95f, 0.95f);
    }

    private void OnDisable()
    {
        if (_playerControls != null)
        {
            _playerControls.Above.Disable();
        }
        _spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    private void FixedUpdate()
    {
        var groundHit = Physics2D.BoxCast(
            transform.position,
            new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y),
            0,
            new Vector2(0, -1),
            transform.localScale.y * 1f,
            _groundMask);
        _onGround = groundHit.collider != null;

        var hardGroundHit = Physics2D.BoxCast(
            transform.position,
            new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y),
            0,
            new Vector2(0, -1),
            transform.localScale.y * 1f,
            _hardGroundMask);
        _onHardGround = hardGroundHit.collider != null;

        var moveInput = _playerControls.Above.Move.ReadValue<Vector2>();

        var moveHit = Physics2D.BoxCast(
            transform.position,
            new Vector2(Mathf.Abs(transform.localScale.x) * 0.9f, transform.localScale.y * 0.9f),
            0,
            moveInput,
            Mathf.Abs(transform.localScale.x) * 0.8f,
            _groundMask | _hardGroundMask
            );

        if (moveHit.collider != null)
        {
            if (_playerSounds.Walking.IsPlaying())
            {
                _playerSounds.Walking.Stop();
            }
            _animationManager.SetWalking(false);
            return;
        }

        var horizontalVelocity = moveInput.x * _moveSpeed;

        _rigidBody.velocity = new Vector2(horizontalVelocity, _rigidBody.velocity.y);

        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (!_playerSounds.Walking.IsPlaying())
            {
                _playerSounds.Walking.Play();
            }
            _animationManager.SetWalking(true);
        }
        else
        {
            if (_playerSounds.Walking.IsPlaying())
            {
                _playerSounds.Walking.Stop();
            }
            _animationManager.SetWalking(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || (!_onGround && !_onHardGround) || IsRooted)
        {
            return;
        }
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpHeight);
        _playerSounds.Jump.Play();
    }

    public void OnRoot(InputAction.CallbackContext context)
    {
        if (!context.performed || !_onGround)
        {
            return;
        }

        IsRooted = true;
        ServiceLocator.Instance.PlayerManager.Root(new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y) - Mathf.Floor(transform.localScale.y)));
    }
}