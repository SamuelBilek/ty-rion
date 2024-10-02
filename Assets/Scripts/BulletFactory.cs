using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    // reference to prefab
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float _bulletRadius;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private float _bulletDelay;

    private float _tmpDelay;
    private Vector3 _startPosition;
    private bool _isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        _tmpDelay = _bulletDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnvironmentalProps.Instance.DeathMode)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _isShooting = true;
            }
            #if UNITY_ANDROID
                _isShooting = true;
            #endif
        }
        if (_isShooting)
        {
            Shoot();
        }
        else
        {
            _tmpDelay = _tmpDelay >= _bulletDelay ? _bulletDelay : _tmpDelay + Time.deltaTime;
        }
        _isShooting = false;
    }

    private void Shoot()
    {
        // time elapsed from previous frame
        _tmpDelay += Time.deltaTime;
        if (_tmpDelay < _bulletDelay)
            return;
        _startPosition = EnvironmentalProps.Instance.GetShipPosition();
        _startPosition.z += EnvironmentalProps.Instance.GetShipScale().z / 2f + 0.25f;

        var bulletGO = Instantiate(bulletPrefab, _startPosition,
                       Quaternion.identity);

        Debug.Log("New bullet spawned at: " + bulletGO.transform.position);
        var bulletContr = bulletGO.GetComponent<BulletController>();
        if (bulletContr != null)
        {
            bulletContr.Set(_bulletSpeed, _bulletRadius);
        }
        else
        {
            Debug.LogError("Missing BulletController component");
        }
        if (_tmpDelay >= _bulletDelay)
            _tmpDelay = 0;
    }
}
