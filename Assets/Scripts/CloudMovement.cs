using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        _moveSpeed = -Random.Range(0.2f, 0.8f);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (_moveSpeed * Time.deltaTime), transform.position.y);

        if (transform.position.x < -100)
        {
            Destroy(gameObject);
        }
    }
}