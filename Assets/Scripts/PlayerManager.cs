using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _abovePlayerPrefab;
    [SerializeField] private GameObject _belowPlayerPrefab;
    private GameObject _abovePlayer;
    private GameObject _belowPlayer;
    private AbovePlayerMovement _aboveMovement;
    private BelowPlayerMovement _belowMovement;
    private CritterMovement _critterMovement;
    private AnimationManager _animationManager;

    private void Awake()
    {
        _abovePlayer = Instantiate(_abovePlayerPrefab);
        _belowPlayer = Instantiate(_belowPlayerPrefab);
        _aboveMovement = _abovePlayer.GetComponent<AbovePlayerMovement>();
        _belowMovement = _belowPlayer.GetComponent<BelowPlayerMovement>();
        _critterMovement = _abovePlayer.GetComponent<CritterMovement>();
        _animationManager = _abovePlayer.GetComponentInChildren<AnimationManager>();

        _critterMovement.enabled = false;
        _belowMovement.enabled = false;
        _belowPlayer.SetActive(false);
        ServiceLocator.Instance.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.6f;

        _abovePlayer.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0.95f, 0.95f);
        _abovePlayer.GetComponent<CapsuleCollider2D>().enabled = false;
        _abovePlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        _abovePlayer.transform.position = new Vector3(-13, 10.8f, -1f);
        ServiceLocator.Instance.Camera.Follow = _abovePlayer.transform;
        StartCoroutine(FallIn(_abovePlayer, _aboveMovement));
    }

    public void Possess(GameObject targetCritter)
    {
        _aboveMovement.gameObject.transform.position =
            new Vector3(_aboveMovement.gameObject.transform.position.x, _aboveMovement.gameObject.transform.position.y, 0);
        _aboveMovement = targetCritter.GetComponent<AbovePlayerMovement>();
        _critterMovement = targetCritter.GetComponent<CritterMovement>();
        _critterMovement.enabled = false;
        _belowMovement.enabled = false;
        ServiceLocator.Instance.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.6f;
        _aboveMovement.gameObject.transform.position =
            new Vector3(_aboveMovement.gameObject.transform.position.x, _aboveMovement.gameObject.transform.position.y, -1f);
        _animationManager = targetCritter.GetComponentInChildren<AnimationManager>();
        if (_aboveMovement.IsRooted)
        {
            _animationManager.Uproot();
        }
        else
        {
            var spriteRenderer = targetCritter.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 0.95f, 0.95f);
            _animationManager.EndPossess();
        }

        if (_aboveMovement.IsGoal)
        {
            StartCoroutine(Win());
            return;
        }
        StartCoroutine(SwitchControl(toAbove: true));
    }

    public void Return()
    {
        _belowMovement.enabled = false;
        ServiceLocator.Instance.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.6f;
        _animationManager.Uproot();
        StartCoroutine(SwitchControl(toAbove: true));
    }

    public void Root(Vector3 position)
    {
        _belowPlayer.SetActive(true);
        _aboveMovement.enabled = false;
        _belowPlayer.transform.position = position;
        ServiceLocator.Instance.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.4f;
        _animationManager.Root();
        StartCoroutine(SwitchControl(toAbove: false));
    }

    private IEnumerator SwitchControl(bool toAbove = false)
    {
        yield return new WaitForSeconds(0.5f);
        if (toAbove)
        {
            _aboveMovement.enabled = true;
        }
        else
        {
            _belowMovement.enabled = true;
        }
    }

    private IEnumerator FallIn(GameObject abovePlayer, AbovePlayerMovement abovePlayerMovement)
    {
        yield return new WaitForSeconds(1);

        _abovePlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        _abovePlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(15, 15);
        _abovePlayer.GetComponent<AbovePlayerSounds>().Jump.Play();

        yield return new WaitForSeconds(0.6f);
        _abovePlayer.GetComponent<CapsuleCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.4f);
        abovePlayerMovement.enabled = true;
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(0.5f);
        _animationManager.Root();
    }
}