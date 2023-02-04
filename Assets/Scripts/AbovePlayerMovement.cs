using UnityEngine;
using UnityEngine.InputSystem;

public class AbovePlayerMovement : MonoBehaviour, PlayerControls.IAboveActions
{
    private PlayerControls _playerControls;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 20f;
    private float _horizontalVelocity;
    private Rigidbody2D _rigidBody;
    private AbovePlayerSounds _playerSounds;
    private int _tilemapMask;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerSounds = GetComponent<AbovePlayerSounds>();
        _tilemapMask = LayerMask.GetMask("Tilemap");
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
    }

    private void OnDisable()
    {
        if (_playerControls != null)
        {
            _playerControls.Above.Disable();
        }
    }

    private void FixedUpdate()
    {
        var groundHit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 1f, _tilemapMask);

        _isGrounded = groundHit.collider != null;

        var moveInput = _playerControls.Above.Move.ReadValue<Vector2>();

        var moveHit = Physics2D.Raycast(transform.position, moveInput, 1f, _tilemapMask);

        if (moveHit.collider != null)
        {
            return;
        }

        _horizontalVelocity = moveInput.x * _moveSpeed;

        _rigidBody.velocity = new Vector2(_horizontalVelocity, _rigidBody.velocity.y);

        if (moveInput.x != 0)
        {
            if (!_playerSounds.Walking.IsPlaying())
            {
                _playerSounds.Walking.Play();
            }
        }
        else
        {
            if (_playerSounds.Walking.IsPlaying())
            {
                _playerSounds.Walking.Stop();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !_isGrounded)
        {
            return;
        }
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpHeight);
        _playerSounds.Jump.Play();
    }

    public void OnRoot(InputAction.CallbackContext context)
    {
        if (transform.position.y > 1f || !_isGrounded)
        {
            return;
        }

        ServiceLocator.Instance.PlayerManager.Switch(new Vector3(Mathf.Round(transform.position.x), -1), toAbove: false);
    }
}