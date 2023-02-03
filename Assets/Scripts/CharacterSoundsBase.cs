using FMODUnity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterSoundsBase : MonoBehaviour
{
    private Dictionary<string, StudioEventEmitter> _emitters;

    private void Awake()
    {
        if (_emitters != null)
        {
            return;
        }

        _emitters = GetSounds().ToDictionary
        (
            x => x.Key, 
            x =>
            {
                var emitter = gameObject.AddComponent<StudioEventEmitter>();
                emitter.EventReference = x.Value;
                return emitter;
            }
        );
    }

    protected StudioEventEmitter GetSound(string name)
    {
        return _emitters[name];
    }

    protected abstract IEnumerable<KeyValuePair<string, EventReference>> GetSounds();
}
