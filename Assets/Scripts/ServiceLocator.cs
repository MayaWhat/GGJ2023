using Cinemachine;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    public MusicController MusicController { get; private set; }
    public CinemachineVirtualCamera Camera { get; private set; }

    public PlayerManager PlayerManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        MusicController = GetComponentInChildren<MusicController>();
        Camera = GetComponentInChildren<CinemachineVirtualCamera>();
        PlayerManager = GetComponentInChildren<PlayerManager>();
    }
}