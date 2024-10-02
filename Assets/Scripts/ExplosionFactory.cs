using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFactory : MonoBehaviour
{
    public static ExplosionFactory Instance { get; private set; }

    void Awake()
    {
        // Check, if we do not have any instance yet.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        {
            // Destroy 'this' object as there exist another instance
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeExplosion(GameObject explosionPrefab, Vector3 position, float size)
    {
        Debug.Log($"Making an explosion!");
        var explosionGO = Instantiate(explosionPrefab, position, Quaternion.identity);
        if (explosionGO != null)
        {
            var explosionContr = explosionGO.GetComponent<ExplosionController>();
            if (explosionContr != null)
            {
                explosionContr.Set(size);
            }
            else
            {
                Debug.LogError("Missing ExplosionController component");
            }
        }
    }
}
