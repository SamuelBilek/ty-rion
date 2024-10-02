using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaBoundaries : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //destroy anything that leaves the play area
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }

}
