using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _abovePlayer;
    [SerializeField] private GameObject _belowPlayer;
    private AbovePlayerMovement _aboveMovement;
    private BelowPlayerMovement _belowMovement;


    private void Awake()
    {
        _aboveMovement = _abovePlayer.GetComponent<AbovePlayerMovement>();
        _belowMovement = _belowPlayer.GetComponent<BelowPlayerMovement>();

        _aboveMovement.enabled = true;
        _belowMovement.enabled = false;
    }

    public void Switch(bool toAbove = false)
    {
        if (toAbove)
        {
            _belowMovement.enabled = false;
            _aboveMovement.enabled = true;

            _abovePlayer.transform.position = _belowPlayer.transform.position;
        }
        else
        {
            _aboveMovement.enabled = false;
            _belowMovement.enabled = true;

            _belowPlayer.transform.position = new Vector3(Mathf.Round(_abovePlayer.transform.position.x), -1);
        }
    }
}