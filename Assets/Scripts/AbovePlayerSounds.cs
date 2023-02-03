using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlayerSounds : CharacterSoundsBase
{
    [SerializeField] private EventReference _jumpSound;
    public StudioEventEmitter Jump => GetSound(nameof(Jump));

    protected override IEnumerable<KeyValuePair<string, EventReference>> GetSounds()
    {
        yield return new KeyValuePair<string, EventReference>(nameof(Jump), _jumpSound);
    }
}