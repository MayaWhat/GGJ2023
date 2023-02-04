using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float _length;
    private float _startPosition;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _parallaxEffect;

    private void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        var temp = (_camera.transform.position.x * (1 - _parallaxEffect));
        var dist = (_camera.transform.position.x * _parallaxEffect);

        transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);

        if (temp > _startPosition + _length)
        {
            _startPosition += _length;
        }
        else if (temp < _startPosition - _length)
        {
            _startPosition -= _length;
        }
    }
}