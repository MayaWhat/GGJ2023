using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : Activator
{
    private List<Collider2D> _activators = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_activators.Count == 0)
        {
            Activate();
        }
        _activators.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _activators.Remove(other);

        if (_activators.Count == 0)
        {
            Deactivate();
        }
    }
}