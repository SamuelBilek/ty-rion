using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickEnabler : MonoBehaviour
{
    private void Awake()
    {
        #if UNITY_STANDALONE_WIN
            Destroy(gameObject);
        #endif
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
