using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class BelowPlayerSounds : CharacterSoundsBase
{
    [SerializeField] private EventReference _burrowSound;
    public StudioEventEmitter Burrow => GetSound(nameof(Burrow));

    [SerializeField] private EventReference _diggingSound;
    public StudioEventEmitter Digging => GetSound(nameof(Digging));

    [SerializeField] private EventReference _unburrowSound;
    public StudioEventEmitter Unburrow => GetSound(nameof(Unburrow));

    [SerializeField] private EventReference _errorSound;
    public StudioEventEmitter Error => GetSound(nameof(Error));

    protected override IEnumerable<KeyValuePair<string, EventReference>> GetSounds()
    {
        yield return new KeyValuePair<string, EventReference>(nameof(Digging), _diggingSound);
        yield return new KeyValuePair<string, EventReference>(nameof(Burrow), _burrowSound);
        yield return new KeyValuePair<string, EventReference>(nameof(Unburrow), _unburrowSound);
        yield return new KeyValuePair<string, EventReference>(nameof(Error), _errorSound);
    }
}