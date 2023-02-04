using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] private Activatee[] _activatees;

    public void Activate()
    {
        foreach (var activatee in _activatees)
        {
            activatee.Activate();
        }
    }

    public void Deactivate()
    {
        foreach (var activatee in _activatees)
        {
            activatee.Deactivate();
        }
    }
}