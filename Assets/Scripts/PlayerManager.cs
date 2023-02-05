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
        _aboveMovement.gameObject.transform.position =
            new Vector3(_aboveMovement.gameObject.transform.position.x, _aboveMovement.gameObject.transform.position.y, -1f);
        StartCoroutine(SwitchControl(toAbove: true));
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
            _animationManager.EndPossess();
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
}