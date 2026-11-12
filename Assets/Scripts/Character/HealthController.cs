using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour, IDamageable
{
    /// <summary>Damage amount, Direction, Knockback Strenght</summary>
    public event UnityAction<float, Vector3, float> OnTakeDamage;
 
    [SerializeField] private float _maxHealth;

    private float _health;

    public float HealthPerCent => _health / _maxHealth;
    public bool CheckFullLife => _health >= _maxHealth;

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        ResetHealth();
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
    }

    public void AddHealth(float bonusHealth)
    {
        _health += bonusHealth;

        if (_health >= _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void TakeDamage(float damage, Vector3 position, float KnockbackStrenght)
    {
        _health -= damage;
    }
}
