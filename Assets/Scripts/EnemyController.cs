using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _speed = 20.0f;
    private float _radius = 1.0f;
    private float _startLeft = 0.0f;
    private float _startRight = 0.0f;
    private bool _goingRight = true;
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float collisionDamage = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // move enemy to the side
        transform.position += new Vector3((_goingRight ? _speed : -_speed) * Time.deltaTime, 0, 0);
        // destroy it on border
        if (EnvironmentalProps.Instance.EscapedBelow(transform.position, _radius + 1))
        {
            Destroy(this.gameObject);
        }

        if (EnvironmentalProps.Instance.EscapedRight(transform.position, _radius))
        {
            transform.position += new Vector3(_startRight - transform.position.x, 0, -_radius * 1.5f);
            _goingRight = false;
        }
        else if (EnvironmentalProps.Instance.EscapedLeft(transform.position, -_radius))
        {
            transform.position += new Vector3(_startLeft - transform.position.x, 0, -_radius * 1.5f);
            _goingRight = true;
        }
    }

    public void Set(float speed, float size)
    {
        _speed = speed;
        _radius = size / 2;
        _startLeft = transform.position.x;
        _startRight = _startLeft + EnvironmentalProps.Instance.sizeX - size;
        transform.localScale = new Vector3(_radius, _radius, _radius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (EnvironmentalProps.Instance.DeathMode)
        {
            return;
        }
        if (health.IsLethal(collisionDamage))
        {
            ExplosionFactory.Instance.MakeExplosion(explosionPrefab, transform.position, _radius / 2);
            EnvironmentalProps.Instance.AddScore(5);
        }
        health.DealDamage(collisionDamage);
    }

    private void OnDestroy()
    {
        EnvironmentalProps.EnemiesCount--;
    }

}
