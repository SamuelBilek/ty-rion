using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private HealthBar healthBar;
    private float _currentHealth;
    private bool _zeroKills = true;
    private float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            healthBar.SetFillLevel(_currentHealth / maxHealth);
            if (_currentHealth <= 0 && _zeroKills)
            {
                Destroy(gameObject);
                // It might be worth considering to call "died" event here :-)
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public bool IsLethal(float damage)
    {
        return (CurrentHealth - damage) <= 0.0f;
    }
    public void DealDamage(float damage)
    {
        CurrentHealth -= damage;
    }
    public void Heal(float heal)
    {
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + heal);
    }
    public void Kill()
    {
        CurrentHealth = 0;
        Debug.Log("You died!");
    }
    public void Revive()
    {
        CurrentHealth = maxHealth;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }
    public void ToggleZeroKills()
    {
        _zeroKills = !_zeroKills;
    }
}
