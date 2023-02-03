using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    public MusicController MusicController { get; private set; }
    public Camera Camera { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        MusicController = GetComponentInChildren<MusicController>();
        Camera = GetComponentInChildren<Camera>();
    }
}
