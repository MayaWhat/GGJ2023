using UnityEngine;
using UnityEngine.InputSystem;

public class AbovePlayerMovement : MonoBehaviour, PlayerControls.IAboveActions
{
    [SerializeField] private PlayerManager _playerManager;
    private PlayerControls _playerControls;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 20f;
    private float _horizontalVelocity;
    private Rigidbody2D _rigidBody;
    private AbovePlayerSounds _playerSounds;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerSounds = GetComponent<AbovePlayerSounds>();
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
        var moveInput = _playerControls.Above.Move.ReadValue<Vector2>();

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
        if (!context.performed || Mathf.Abs(_rigidBody.velocity.y) > 0.1f)
        {
            return;
        }
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpHeight);
        _playerSounds.Jump.Play();
    }

    public void OnRoot(InputAction.CallbackContext context)
    {
        _playerManager.Switch(new Vector3(Mathf.Round(transform.position.x), -1), toAbove: false);
    }
}