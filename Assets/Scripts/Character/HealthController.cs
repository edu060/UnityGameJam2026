using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour, IDamageable
{
    /// <summary>Damage amount, Direction, Knockback Strenght</summary>
    public event UnityAction<float, Vector3, float> OnTakeDamage;
    public UnityAction OnDie;
    public UnityAction OnLoseHealth;
    public UnityAction OnGainHealth;


    [SerializeField] private float _maxHealth;

    private float _health;

    public float HealthPerCent => _health / _maxHealth;
    public bool CheckFullLife => _health >= _maxHealth;

    private void Start()
    {
        ResetHealth();
    }

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
        OnGainHealth?.Invoke();
    }

    public void TakeDamage(float damage, Vector3 position, float KnockbackStrenght)
    {
        Debug.Log(damage);
        _health -= damage;

        if(_health <= 0)
        {
            Die();
        }
        else
        {
            OnLoseHealth?.Invoke();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
    }
}
