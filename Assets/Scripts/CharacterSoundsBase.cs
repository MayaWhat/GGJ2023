using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSoundsBase : MonoBehaviour
{
    private Dictionary<string, StudioEventEmitter> _emitters = new Dictionary<string, StudioEventEmitter>();

    private void Start()
    {
        foreach (var sound in GetSounds())
        {
            var emitter = gameObject.AddComponent<StudioEventEmitter>();
            emitter.EventReference = sound.Value;
            _emitters.Add(sound.Key, emitter);
        }
    }

    protected Action GetSound(string name)
    {
        return _emitters[name].Play;
    }

    protected abstract IEnumerable<KeyValuePair<string, EventReference>> GetSounds();
}
