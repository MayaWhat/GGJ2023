using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class BelowPlayerSounds : CharacterSoundsBase
{
    [SerializeField] private EventReference _burrowSound;
    public StudioEventEmitter Burrow => GetSound(nameof(Burrow));

    [SerializeField] private EventReference _diggingSound;
    public StudioEventEmitter Digging => GetSound(nameof(Digging));

    protected override IEnumerable<KeyValuePair<string, EventReference>> GetSounds()
    {
        yield return new KeyValuePair<string, EventReference>(nameof(Digging), _diggingSound);
        yield return new KeyValuePair<string, EventReference>(nameof(Burrow), _burrowSound);
    }
}