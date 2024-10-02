using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    // reference to prefab
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float _enemySize;
    [SerializeField]
    private float _enemyDelay;
    [SerializeField]
    private float _enemySpeed;
    [SerializeField]
    private float _maxCount;
    // delay from last spawn
    private float _delay;

    // Start is called before the first frame update
    void Start()
    {
        _delay = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // time elapsed from previous frame
        _delay -= Time.deltaTime;
        if (_delay > 0.0f)
            return;
        if (EnvironmentalProps.EnemiesCount >= _maxCount)
            return;
        //choose position for new spawn
        //horizontal
        float x = EnvironmentalProps.Instance.minX() + _enemySize / 2;
        //vertical
        float z = EnvironmentalProps.Instance.maxZ() + _enemySize / 2;
        // create new instance of prefab at given position
        var enemyGO = Instantiate(_enemyPrefab, new Vector3(x, 0, z),
                       Quaternion.identity);
        EnvironmentalProps.EnemiesCount++;

        Debug.Log("New enemy spawned at: " + enemyGO.transform.position);
        var enemyContr = enemyGO.GetComponent<EnemyController>();
        if (enemyContr != null)
        {
            enemyContr.Set(_enemySpeed, _enemySize);
        }
        else
        {
            Debug.LogError("Missing EnemyController component");
        }
        // set new delay for next spawn
        _delay = _enemyDelay;
    }
}
