using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    [SerializeField] private List<Sprite> _cloudSprites;
    [SerializeField] private GameObject _cloudPrefab;
    private List<GameObject> _clouds = new List<GameObject>();
    [SerializeField] private int _cloudSpawnDeviation = 5;
    [SerializeField] private int _cloudSpawnFrequency = 10;
    private int _cloudTimer = 0;
    private int _lastCloudSector = 0;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _parallaxEffect;
    private float _startPosition;

    private void Awake()
    {
        _startPosition = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (_cloudTimer <= 0)
        {
            var cloudY = Random.Range(4, 34);
            if (cloudY / 10 == _lastCloudSector)
            {
                cloudY += 10;
            }
            _lastCloudSector = cloudY / 10;
            GameObject cloud = Instantiate(_cloudPrefab, new Vector3(_camera.transform.position.x + 20f, cloudY), new Quaternion());
            cloud.GetComponent<SpriteRenderer>().sprite = _cloudSprites[Random.Range(0, _cloudSprites.Count)];
            _clouds.Add(cloud);

            _cloudTimer = _cloudSpawnFrequency + Random.Range(-_cloudSpawnDeviation, _cloudSpawnDeviation);
        }
        else
        {
            _cloudTimer--;
        }
    }

    private void Update()
    {
        var dist = (_camera.transform.position.x * _parallaxEffect);

        transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);
    }
}