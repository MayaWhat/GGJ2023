using FMODUnity;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private const string DARK_PARAM = "Dark";

    private StudioEventEmitter _emitter;


    private void Awake()
    {
        _emitter = GetComponent<StudioEventEmitter>();

        // We probably want to do something to actually trigger the music eventually but for now just kick it off right away
        Play();
    }

    public void Play()
    {
        _emitter.Play();
    }

    public void Stop()
    {
        _emitter.Stop();
    }

    public void Dark()
    {
        _emitter.SetParameter(DARK_PARAM, 1f);
    }

    public void Happy()
    {
        _emitter.SetParameter(DARK_PARAM, 0f);
    }
}
