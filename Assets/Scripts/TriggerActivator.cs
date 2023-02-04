using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : Activator
{
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private Sprite _deactivatedSprite;

    private SpriteRenderer _renderer;

    private List<Collider2D> _activators = new List<Collider2D>();

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_activators.Count == 0)
        {
            _renderer.sprite = _activatedSprite;
            Activate();
        }
        _activators.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _activators.Remove(other);

        if (_activators.Count == 0)
        {
            _renderer.sprite = _deactivatedSprite;
            Deactivate();
        }
    }
}