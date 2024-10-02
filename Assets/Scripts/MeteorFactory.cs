using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFactory : MonoBehaviour
{
    // reference to prefab
    [SerializeField]
    private GameObject meteorPrefab;
    [SerializeField]
    private float delayMin;
    [SerializeField]
    private float delayMax;
    [SerializeField]
    private float _minRadius;
    [SerializeField]
    private float _maxRadius;
    [SerializeField]
    private float _minSpeed;
    // delay from last spawn
    private float _delay;


    void Start()
    {
        _delay = 0;
    }

    void Update()
    {
        // time elapsed from previous frame
        _delay -= Time.deltaTime;
        if (_delay > 0.0f)
            return;
        //choose position for new spawn
        //horizontal
        float meteorRadius = Random.Range(_minRadius, _maxRadius);
        float x = Random.Range(
            EnvironmentalProps.Instance.minX() + meteorRadius,
            EnvironmentalProps.Instance.maxX() - meteorRadius
        );
        float meteorSpeed = _minSpeed * (_maxRadius - meteorRadius + 1);
        //vertical
        float z = EnvironmentalProps.Instance.maxZ() + meteorRadius + 5;
        // create new instance of prefab at given position
        var meteorGO = Instantiate(meteorPrefab, new Vector3(x, 0, z),
                       Quaternion.identity);

        //LATER - set speed and size of meteor
        Debug.Log("New meteor spawned at: " + meteorGO.transform.position);
        var meteorContr = meteorGO.GetComponent<MeteorController>();
        if (meteorContr != null)
        {
            meteorContr.Set(meteorSpeed, meteorRadius);
        }
        else
        {
            Debug.LogError("Missing MeteorController component");
        }
        // set new delay for next spawn
        _delay = Random.Range(delayMin, delayMax);
    }

}
