using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    [SerializeField] private List<Sprite> _cloudSprites;
    [SerializeField] private GameObject _cloudPrefab;
    private float _levelWidth = 100f;
    private List<GameObject> _clouds = new List<GameObject>();
    private int _cloudSpawnDeviation = 200;
    private int _cloudSpawnFrequency = 1000;
    private int _cloudTimer = 0;
    private int _lastCloudSector = 0;
    private int _initialClouds = 20;
    [SerializeField] private GameObject _camera;
    private float _parallaxEffect = 0.9f;
    private float _startPosition;

    private void Awake()
    {
        _startPosition = transform.position.x;
    }

    private void OnEnable()
    {
        var seperation = _levelWidth / _initialClouds;

        for (int i = 0; i < _initialClouds; i++)
        {
            SpawnCloud(i * seperation);
        }
    }

    private void FixedUpdate()
    {
        if (_cloudTimer <= 0)
        {
            SpawnCloud(_levelWidth);
            _cloudTimer = _cloudSpawnFrequency + Random.Range(-_cloudSpawnDeviation, _cloudSpawnDeviation);
        }
        else
        {
            _cloudTimer--;
        }
    }

    private void SpawnCloud(float xPosition)
    {
        var cloudY = Random.Range(6, 25);
        if (cloudY / 10 == _lastCloudSector)
        {
            cloudY -= 5;
        }
        _lastCloudSector = cloudY / 10;

        GameObject cloud = Instantiate(_cloudPrefab, new Vector3(xPosition, cloudY), new Quaternion());
        cloud.GetComponent<SpriteRenderer>().sprite = _cloudSprites[Random.Range(0, _cloudSprites.Count)];
        _clouds.Add(cloud);
        cloud.transform.parent = transform;
    }

    private void Update()
    {
        var dist = (_camera.transform.position.x * _parallaxEffect);

        transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);
    }
}