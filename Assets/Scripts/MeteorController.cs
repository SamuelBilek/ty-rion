using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    private float _speed = 20.0f;
    private float _radius = 1.0f;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private Health health;
    [SerializeField]
    private float collisionDamage = 20.0f;

    void Update()
    {
        // move meteor down
        transform.position += new Vector3(0, 0, -_speed * Time.deltaTime);
        // destroy it on border
        if (EnvironmentalProps.Instance.EscapedBelow(transform.position, _radius + 1))
        {
            StartDestruction();
        }
    }

    private void StartDestruction()
    {
        Destroy(this.gameObject, 6.0f);
    }

    public void Set(float speed, float radius)
    {
        _speed = speed;
        _radius = radius;
        transform.localScale = new Vector3(_radius, _radius, _radius);
        collisionDamage = collisionDamage / radius;
        Transform model = this.transform.Find("Visuals");
        if (model != null)
        {
            model.rotation = Random.rotation;
        }
        else
        {
            Debug.Log("Didn't work!");
        }
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
            EnvironmentalProps.Instance.AddScore(10);
        }
        health.DealDamage(collisionDamage);

    }

    private void OnDestroy()
    {
        
    }


}
