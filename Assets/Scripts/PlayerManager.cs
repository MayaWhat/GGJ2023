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

    private void Awake()
    {
        _abovePlayer = Instantiate(_abovePlayerPrefab);
        _belowPlayer = Instantiate(_belowPlayerPrefab);
        _aboveMovement = _abovePlayer.GetComponent<AbovePlayerMovement>();
        _belowMovement = _belowPlayer.GetComponent<BelowPlayerMovement>();
        _critterMovement = _abovePlayer.GetComponent<CritterMovement>();

        _critterMovement.enabled = false;
        _aboveMovement.enabled = true;
        _belowMovement.enabled = false;
        _belowPlayer.SetActive(false);
    }

    public void Possess(GameObject targetCritter)
    {
        _aboveMovement = targetCritter.GetComponent<AbovePlayerMovement>();
        _critterMovement = targetCritter.GetComponent <CritterMovement>();
        _critterMovement.enabled = false;
        _aboveMovement.enabled = true;
        _belowMovement.enabled = false;
    }

    public void Return()
    {
        _belowMovement.enabled = false;
        _aboveMovement.enabled = true;
    }

    public void Root(Vector3 position)
    {
        _belowPlayer.SetActive(true);
        _aboveMovement.enabled = false;
        _belowMovement.enabled = true;
        _belowPlayer.transform.position = position;
    }
}