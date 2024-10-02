using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShip : MonoBehaviour
{
    enum State
    {
        ENTER_GAME_ZONE,
        SHORT_ATTACK,
        LONG_ATTACK,
        SUPER_ATTACK,
        RETREAT
    }

    [SerializeField]
    private float _secondPhaseHpThreshold;
    [SerializeField]
    private GameObject _shortProjectile;
    [SerializeField]
    private GameObject _longProjectile;
    [SerializeField]
    private GameObject _superProjectile;

    private State activeState = State.ENTER_GAME_ZONE;
    public float minReactionDelay = 0.1f;
    public float maxReactionDelay = 0.2f;
    private float reactionDelay = 0.0f;

    private bool gameZoneEntered = false;
    public int NumShortShotsToCooldown = 7;
    private int numShortShots = 0;

    public int NumLongShotsToCooldown = 5;
    private int numLongShots = 0;

    public int NumSuperShotsToCooldown = 5;

    public float maxAccel = 20.0f;
    public float maxSpeed = 5.0f;
    private Vector3 velocity = Vector3.zero;

    public float ShortFirePointShiftZ = 6.0f;
    public float LongFirePointShiftZ = 13.0f;
    public float SuperFirePointShiftZ = 9.0f;
    private Vector3 enemyPosition = Vector3.zero;

    public float ShortReloadSeconds = 0.3f;
    public float LongReloadSeconds = 0.8f;
    public float SuperReloadSeconds = 15.0f;

    private float shortReload = 0.0f;
    private float longReload = 0.0f;
    private float superReload = 0.0f;

    public float CooldownSeconds = 5.0f;
    private float cooldown;
    private Transform gun;

    [SerializeField]
    private float collisionDamage;
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private AudioClip _shieldSound;
    [SerializeField]
    private AudioClip _secondPhaseSound;

    private bool _secondPhase = false;
    private bool _shieldMode = true;




    // Start is called before the first frame update
    void Start()
    {
        gun = transform.Find("Gun");
        cooldown = CooldownSeconds;

    }

    // Update is called once per frame
    void Update()
    {
        if (EnvironmentalProps.Instance.VictoryMode)
        {
            StartCoroutine(VictoryCoroutine());
            return;
        }
        if (!_secondPhase && health.GetCurrentHealth() <= health.GetMaxHealth() * _secondPhaseHpThreshold)
        {
            SecondPhase();
            _secondPhase = true;
            SoundManager.Instance.PlaySound(_secondPhaseSound);
        }
        reactionDelay -= Time.deltaTime;
        if (reactionDelay <= 0.0f)
        {
            reactionDelay = Random.Range(minReactionDelay, maxReactionDelay);
            ScanEnvironment();
            SelectState();
        }
        ProcessGunTimers();
        switch (activeState)
        {
            case State.ENTER_GAME_ZONE:
                ShieldMode(true);
                Process_ENTER_GAME_ZONE();
                break;
            case State.SUPER_ATTACK:
                ShieldMode(false);
                Process_SUPER_ATTACK();
                break;
            case State.LONG_ATTACK:
                ShieldMode(false);
                Process_LONG_ATTACK();
                break;
            case State.SHORT_ATTACK:
                ShieldMode(false);
                Process_SHORT_ATTACK();
                break;
            case State.RETREAT:
                ShieldMode(true);
                Process_RETREAT();
                break;
            default: Debug.Assert(false); break;
        }

    }

    private void ScanEnvironment()
    {
        enemyPosition = EnvironmentalProps.Instance.GetShipPosition();
    }

    private void ProcessGunTimers()
    {
        if (superReload > 0.0f)
            superReload -= Time.deltaTime;
        if (activeState == State.RETREAT)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                cooldown = CooldownSeconds;

                shortReload = 0.0f;
                longReload = 0.0f;

                numShortShots = 0;
                numLongShots = 0;
            }
            return;
        }
        if (shortReload > 0.0f)
            shortReload -= Time.deltaTime;
        if (longReload > 0.0f)
            longReload -= Time.deltaTime;
    }

    private void ShortShoot()
    {
        if (shortReload <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            BossProjectile.Instantiate(
            _shortProjectile, 
            gun.position,
            horizontalVelocity,
            Matrix4x4.Rotate(gun.rotation).MultiplyVector(
            new Vector3(0, 0, 1)
            )
            );
            ++numShortShots;
            shortReload = ShortReloadSeconds;
            Debug.Log("Short Range Attack");
        }
    }

    private void LongShoot()
    {
        if (longReload <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            BossProjectile.Instantiate(
            _longProjectile, 
            gun.position,
            horizontalVelocity,
            Matrix4x4.Rotate(gun.rotation).MultiplyVector(
            new Vector3(0, 0, 1)
            )
            );
            ++numLongShots;
            longReload = LongReloadSeconds;
            Debug.Log("Long Range Attack");
        }
    }

    private void SuperShoot()
    {
        if (superReload <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Vector3 aim = Quaternion.Euler(0.0f, -45.0f, 0.0f) * Vector3.left;
            for (int i = 0; i < NumSuperShotsToCooldown; i++)
            {
                BossProjectile.Instantiate(_superProjectile, gun.position,horizontalVelocity, aim);
                aim = Quaternion.Euler(0.0f, 90.0f / -NumSuperShotsToCooldown, 0.0f) * aim;
            }
            superReload = SuperReloadSeconds;
            Debug.Log("Super Attack");
        }
    }

    public void SecondPhase()
    {
        ShortReloadSeconds /= 1.5f;
        LongReloadSeconds /= 1.5f;
        SuperReloadSeconds /= 1.5f;
        CooldownSeconds /= 2.0f;
    }

    private void SelectState()
    {
        if (!gameZoneEntered)
            activeState = State.ENTER_GAME_ZONE;
        else if (enemyPosition.z >= 0.0 || 
                 numShortShots < NumShortShotsToCooldown)
            activeState = State.SHORT_ATTACK;
        else if (superReload <= 0.0f)
            activeState = State.SUPER_ATTACK;
        else if (numLongShots < NumLongShotsToCooldown)
            activeState = State.LONG_ATTACK;
        else if (numShortShots < NumShortShotsToCooldown)
            activeState = State.SHORT_ATTACK;
        else
            activeState = State.RETREAT;
    }
    private void Process_ENTER_GAME_ZONE()
    {
        EnvironmentalProps env = EnvironmentalProps.Instance;
        Vector3 target = new Vector3(0.5f * (env.minX() + env.maxX()),0.0f,env.minZ() + 0.75f * (env.maxZ() - env.minZ()));
        velocity = EnvironmentalProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel,target, Time.deltaTime);
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        if ((target - transform.position).sqrMagnitude < 1.0f)
            gameZoneEntered = true;
    }
    private void Process_SHORT_ATTACK()
    {
        velocity = EnvironmentalProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel, enemyPosition + new Vector3(0, 0, ShortFirePointShiftZ),Time.deltaTime);
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentalProps.Instance.IntoArea(transform.position);
        if ((enemyPosition - transform.position).magnitude <= ShortFirePointShiftZ + 2 && 
            (enemyPosition - transform.position).magnitude >= ShortFirePointShiftZ - 2 &&
            !EnvironmentalProps.Instance.isOutsideGameArea(transform.position))
        {
            ShortShoot();
        }
    }

    private void Process_LONG_ATTACK()
    {
        velocity = EnvironmentalProps.ComputeSeekVelocity(transform.position, velocity, maxSpeed, maxAccel, enemyPosition + new Vector3(0, 0, LongFirePointShiftZ), Time.deltaTime);
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentalProps.Instance.IntoArea(transform.position);
        if ((enemyPosition - transform.position).magnitude <= LongFirePointShiftZ + 2 && 
            (enemyPosition - transform.position).magnitude >= LongFirePointShiftZ - 2 &&
            !EnvironmentalProps.Instance.isOutsideGameArea(transform.position, - 2))
        {
            LongShoot();
        }
    }

    private void Process_SUPER_ATTACK()
    {
        velocity = EnvironmentalProps.ComputeSeekVelocity(transform.position, velocity, maxSpeed, maxAccel, enemyPosition + new Vector3(0, 0, SuperFirePointShiftZ), Time.deltaTime);
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentalProps.Instance.IntoArea(transform.position);
        if ((enemyPosition - transform.position).magnitude <= SuperFirePointShiftZ + 2 && 
            (enemyPosition - transform.position).magnitude >= SuperFirePointShiftZ - 2 &&
            !EnvironmentalProps.Instance.isOutsideGameArea(transform.position, - 2))
        {
            SuperShoot();
        }
    }
    private void Process_RETREAT()
    {
        Vector3 target = new Vector3(EnvironmentalProps.Instance.minX() +(enemyPosition.x < transform.position.x ? 0.8f : 0.2f) *(EnvironmentalProps.Instance.maxX() - EnvironmentalProps.Instance.minX()),0, EnvironmentalProps.Instance.minZ() +1.1f *(EnvironmentalProps.Instance.maxZ() - EnvironmentalProps.Instance.minZ()));
        velocity = EnvironmentalProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel,target, Time.deltaTime);
        transform.position = EnvironmentalProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentalProps.Instance.IntoArea(transform.position, - 2);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (EnvironmentalProps.Instance.VictoryMode)
        {
            return;
        }
        if (_shieldMode)
        {
            return;
        }
        if (health.IsLethal(collisionDamage))
        {
            EnvironmentalProps.Instance.AddScore(2000);
            ExplosionFactory.Instance.MakeExplosion(explosionPrefab, transform.position, transform.localScale.x * 2);
            EnvironmentalProps.Instance.VictoryMode = true;
            var meshes = GetComponentsInChildren<Renderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = false;
            }
            var capsules = GetComponentsInChildren<CapsuleCollider>();
            foreach (var c in capsules)
            {
                c.enabled = false;
            }

            return;
        }
        health.DealDamage(collisionDamage);
    }

    public void ShieldMode(bool turnedOn)
    {
        if (_shieldMode != turnedOn)
            SoundManager.Instance.PlaySound(_shieldSound);
        var shield = transform.Find("Visuals").Find("Shield");
        shield.GetComponent<Renderer>().enabled = turnedOn;
        var sphere = GetComponentInChildren<SphereCollider>();
        sphere.enabled = turnedOn;
        _shieldMode = turnedOn;
    }

    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.StopAlarm();
        LevelsManager.Victory();
    }

}
