using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    //array of projectile prefabs
    [SerializeField]
    private GameObject[] prefabs = new GameObject[3];
    // delay from last shot
    private float _delay;

    // Start is called before the first frame update
    void Start()
    {
        _delay = Random.Range(EnvironmentalProps.Instance.EnemyShootingMinDelay, EnvironmentalProps.Instance.EnemyShootingMaxDelay);
    }

    // Update is called once per frame
    void Update()
    {
        _delay -= Time.deltaTime;
        if (_delay > 0.0f)
            return;
        //note - with floats parameters, Random.Range() is inclusive...
        //but for integers - check yourself
        int prefabIndex = Random.Range(0, 3);
        //instantiate prefab, with same orientation as gun itself
        Instantiate(prefabs[prefabIndex], transform.position, transform.rotation);
        //reset delay
        _delay = Random.Range(EnvironmentalProps.Instance.EnemyShootingMinDelay, EnvironmentalProps.Instance.EnemyShootingMaxDelay);
    }
}
