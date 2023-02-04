using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _abovePlayerPrefab;    
    [SerializeField] private GameObject _belowPlayerPrefab;
    private GameObject _abovePlayer;
    private GameObject _belowPlayer;
    private AbovePlayerMovement _aboveMovement;
    private BelowPlayerMovement _belowMovement;

    private void Awake()
    {
        _abovePlayer = Instantiate(_abovePlayerPrefab);
        _belowPlayer = Instantiate(_belowPlayerPrefab);
        _aboveMovement = _abovePlayer.GetComponent<AbovePlayerMovement>();
        _belowMovement = _belowPlayer.GetComponent<BelowPlayerMovement>();

        _aboveMovement.enabled = true;
        _belowMovement.enabled = false;
        _belowPlayer.SetActive(false);
    }

    public void Switch(Vector3? position = null, bool toAbove = false)
    {
        if (toAbove)
        {
            _belowMovement.enabled = false;
            _aboveMovement.enabled = true;

            if (position.HasValue)
            {
                _abovePlayer.transform.position = position.Value;
            }
        }
        else
        {
            _belowPlayer.SetActive(true);
            _aboveMovement.enabled = false;
            _belowMovement.enabled = true;

            if (position.HasValue)
            {
                _belowPlayer.transform.position = position.Value;
            }
        }
    }
}