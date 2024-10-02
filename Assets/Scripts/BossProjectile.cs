using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private const float speed = 10.0f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        if (EnvironmentalProps.Instance.isOutsideGameArea(transform.position, 25.0f))
            Destroy(gameObject);
    }

    public static BossProjectile Instantiate(GameObject projectile, Vector3 pos, Vector3 gunVelocity, Vector3 gutUnitAimingDir)
    {
        // First we create a default sphere GO.
        GameObject sphere = Instantiate(projectile);
        BossProjectile self = sphere.AddComponent<BossProjectile>();
        // And we set the position and velocity
        self.transform.position = pos;
        self.velocity = gunVelocity + speed * gutUnitAimingDir;
        return self;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
