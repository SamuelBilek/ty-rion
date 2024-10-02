using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    public float speed = 5f;

    private CapsuleCollider _collider;
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float collisionDamage = 40.0f;
    [SerializeField]
    private AudioClip _destroySound;
    [SerializeField]
    private AudioClip _damageSound;
    [SerializeField]
    private Joystick _joystick;

    private bool _criticalDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        health.ToggleZeroKills();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnvironmentalProps.Instance.DeathMode)
        {
            StartCoroutine(DeathCoroutine());
        }
        Vector3 scale = transform.localScale;
        Vector3 velocity = new Vector3(0, 0, 0);

        // Check, if the 'A' key is pressed
        #if UNITY_ANDROID
            if (_joystick?.Horizontal <= -0.2f)
                velocity.x -= speed;
            if (_joystick?.Horizontal >= 0.2f)
                velocity.x += speed;
            if (_joystick?.Vertical <= -0.2f)
                velocity.z -= speed;
            if (_joystick?.Vertical >= 0.2f)
                velocity.z += speed;
        #endif

        if (Input.GetKey(KeyCode.A))
            velocity.x -= speed;
        if (Input.GetKey(KeyCode.D))
            velocity.x += speed;
        if (Input.GetKey(KeyCode.S))
            velocity.z -= speed;
        if (Input.GetKey(KeyCode.W))
            velocity.z += speed;
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentalProps.Instance.IntoArea(transform.position, _collider.radius);


        EnvironmentalProps.Instance.SetShipPosition(transform.position);
        EnvironmentalProps.Instance.SetShipScale(scale);
        EnvironmentalProps.Instance.AddScore(0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (EnvironmentalProps.Instance.DeathMode)
        {
            return;
        }
        if (health.IsLethal(collisionDamage))
        {
            health.DealDamage(collisionDamage);
            ExplosionFactory.Instance.MakeExplosion(explosionPrefab, transform.position, transform.localScale.x);
            EnvironmentalProps.Instance.DeathMode = true;
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            SoundManager.Instance.StopAlarm();
            SoundManager.Instance.PlaySound(_destroySound);
            return;
        }
        SoundManager.Instance.PlaySound(_damageSound);
        health.DealDamage(collisionDamage);
        if (!_criticalDamage && health.GetCurrentHealth() <= health.GetMaxHealth() / 5.0f)
        {
            _criticalDamage = true;
            SoundManager.Instance.Alarm();
        }
    }
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Death Screen");
    }
}
