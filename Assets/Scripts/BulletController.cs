using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed = 0f;
    private float _radius = 0f;

    // Update is called once per frame
    void Update()
    {
        // move meteor down
        transform.position += new Vector3(0, 0, _speed * Time.deltaTime);
        // destroy it on border
        if (EnvironmentalProps.Instance.EscapedAbove(transform.position, -_radius - 1))
        {
            Destroy(this.gameObject);
        }
    }

    public void Set(float speed, float radius)
    {
        _speed = speed;
        _radius = radius;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
