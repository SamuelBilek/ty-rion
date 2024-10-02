using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(float particleSize)
    {
        ParticleSystem explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
        var main = explosion.main;
        main.startSize = particleSize;
        explosion = transform.GetChild(1).GetComponent<ParticleSystem>();
        main = explosion.main;
        main.startSize = particleSize;
    }
}
