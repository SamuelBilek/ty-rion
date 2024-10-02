using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // move projectile "forward"
        transform.localPosition -= transform.forward *
        EnvironmentalProps.Instance.ProjectileSpeed * Time.deltaTime;

        if (EnvironmentalProps.Instance.EscapedBelow(transform.position, 3))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
