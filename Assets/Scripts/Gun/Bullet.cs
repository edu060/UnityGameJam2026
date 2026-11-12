using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [Header("Components")]
    [SerializeField] private Collider _bulletCollider;

    private float _speed;
    private float _damage = 5f;
    private float _knockbackStrenght;

    private bool _isExplosive = false;
    private float _explosionSize = 5f;
    /// <summary>Set between 0 to 1</summary>
    private float _explosionExtraDamagePercent = 0.5f;
    private float _explosionKnockbackStrenght;

    private bool _isRicochetable = false;
    private int _maxRicocheteAmount = 3;
    private int _currentRicocheteAmount;

    private float _lifeTime = 10f;

    private Collider _ownerCollider;

    private Coroutine _despawnCoroutine;

    public void SetOwner(Collider ownerCollider)
    {
        _ownerCollider = ownerCollider;

        if (_ownerCollider != null)
        {
            Physics.IgnoreCollision(_bulletCollider, _ownerCollider, true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        HitSomething(collision);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetExplosive(bool isExplosive)
    {
        _isExplosive = isExplosive;
    }

    public void SetRicochetable(bool isRicochetable)
    {
        _isRicochetable = isRicochetable;
    }

    public void OnSpawned()
    {
        _currentRicocheteAmount = 0;

        if (_despawnCoroutine != null)
            StopCoroutine(_despawnCoroutine);

        _despawnCoroutine = StartCoroutine(Despawn());
    }

    public void OnDespawned()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }

        if (_ownerCollider != null)
            Physics.IgnoreCollision(_bulletCollider, _ownerCollider, false);

        _speed = 0;

        _isExplosive = false;

        _isRicochetable = false;
        _currentRicocheteAmount = 0;

        _ownerCollider = null;

        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(_lifeTime);

        ObjectPoolManager.Instance.Despawn(gameObject);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
    }

    private void HitSomething(Collision collision)
    {
        IDamageable damageable =
            collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(
                _damage,
                transform.position,
                _knockbackStrenght);
        }

        if (_isExplosive)
        { 
            Explode(collision.contacts[0].point); 
        }

        if (_isRicochetable && _currentRicocheteAmount < _maxRicocheteAmount)
        {
            Ricochet(collision);
            return;
        }

        ObjectPoolManager.Instance.Despawn(gameObject);
    }

    private void Explode(Vector3 explosionPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            explosionPosition,
            _explosionSize
        );

        foreach (Collider hitCollider in hitColliders)
        {
            if (_ownerCollider != null &&
                hitCollider == _ownerCollider)
                continue;

            IDamageable damageable =
                hitCollider.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(
                    _damage * _explosionExtraDamagePercent, 
                    transform.position, 
                    _explosionKnockbackStrenght);
            }
        }
    }

    private void Ricochet(Collision collision)
    {
        Vector3 incomingDirection = transform.forward;
        Vector3 surfaceNormal = collision.contacts[0].normal;

        Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, surfaceNormal).normalized;

        _currentRicocheteAmount++;

        transform.rotation = Quaternion.LookRotation(reflectedDirection);

        transform.position += surfaceNormal * 0.05f;
    }

    private void Update()
    {
        MoveForward();
    }
}
