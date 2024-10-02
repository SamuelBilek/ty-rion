using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentalProps : MonoBehaviour
{
    public static EnvironmentalProps Instance { get; private set; }
    public static float EnemiesCount { get; set; }

    //delay between two shots from of geometry gun
    [SerializeField]
    private float _enemyShootingMinDelay;
    [SerializeField]
    private float _enemyShootingMaxDelay;
    public float EnemyShootingMinDelay { get { return _enemyShootingMinDelay; } }
    public float EnemyShootingMaxDelay { get { return _enemyShootingMaxDelay; } }

    public bool DeathMode = false;
    public bool VictoryMode = false;

    // speed of all projectiles in game
    [SerializeField]
    private float projectileSpeed = 5.0f;
    public float ProjectileSpeed { get { return projectileSpeed; } }

    public float sizeX;
    public float sizeZ;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    private static int _scoreValue = 0;

    public void AddScore(int score)
    {
        _scoreValue += score;
        textMeshProUGUI.text = "SCORE: " + _scoreValue.ToString();
    }

    public void PrintFinalScore()
    {
        textMeshProUGUI.text = "Final Score: " + _scoreValue.ToString();
    }

    public int GetScore()
    {
        return _scoreValue;
    }

    public void SetScore(int score)
    {
        _scoreValue = score;
    }

    private Vector3 _shipPosition = new Vector3();
    private Vector3 _shipScale = new Vector3();

    public float minX() { return -sizeX / 2.0f; }
    public float maxX() { return sizeX / 2.0f; }
    public float minZ() { return -sizeZ / 2.0f; }
    public float maxZ() { return sizeZ / 2.0f; }



    void Awake()
    {
        // Check, if we do not have any instance yet.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy 'this' object as there exist another instance
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">pozice</param>
    /// <param name="dx">radius/polomer</param>
    /// <returns></returns>
    public Vector3 IntoArea(Vector3 pos, float dx = 0f)
    {
        Vector3 result = pos;
        result.x = result.x - dx < minX() ? minX() + dx : result.x;
        result.x = result.x + dx > maxX() ? maxX() - dx : result.x;
        result.z = result.z - dx < minZ() ? minZ() + dx : result.z;
        result.z = result.z + dx > maxZ() ? maxZ() - dx : result.z;
        return result;
    }

    public bool EscapedBelow(Vector3 pos, float dz)
    {
        return pos.z + dz < minZ();
    }

    public bool EscapedAbove(Vector3 pos, float dz)
    {
        return pos.z + dz > maxZ();
    }

    public bool EscapedRight(Vector3 pos, float dx)
    {
        return pos.x + dx > maxX();
    }

    public bool EscapedLeft(Vector3 pos, float dx)
    {
        return pos.x + dx < minX();
    }

    public bool isOutsideGameArea(Vector3 pos, float extend = 0)
    {
        return pos.x < minX() - extend || pos.x > maxX() + extend || pos.z < minZ() - extend || pos.z >maxZ() + extend;
    }

    public void SetShipPosition(Vector3 pos)
    {
        _shipPosition = pos;
    }

    public Vector3 GetShipPosition()
    {
        return _shipPosition;
    }

    public void SetShipScale(Vector3 scale)
    {
        _shipScale = scale;
    }

    public Vector3 GetShipScale()
    {
        return _shipScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 ComputeEulerStep(Vector3 x0, Vector3 dx_dt, float delta_t)
    {
        return x0 + delta_t * dx_dt;
    }

    public static Vector3 ComputeSeekAccel(Vector3 pos, float maxAccel, Vector3 targetPos)
    {
        Vector3 dir = targetPos - pos;
        // NOTE: We add 1e-6f to handle the case when pos == targetPos.
        return (maxAccel / (dir.magnitude + 1e-6f)) * dir;
    }
    public static Vector3 ComputeSeekVelocity(Vector3 pos, Vector3 velocity,float maxSpeed, float maxAccel,Vector3 targetPos, float dt)
    {
        Vector3 seek_accel = ComputeSeekAccel(pos, maxAccel, targetPos);
        velocity = ComputeEulerStep(velocity, seek_accel, dt);
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            velocity *= (maxSpeed / velocity.magnitude);
        return velocity;
    }

}
