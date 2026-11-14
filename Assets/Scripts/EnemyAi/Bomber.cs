using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Image;

public class Bomber : Enemy
{
    private enum State { 
        Idle, 
        Chasing, 
        Fusing, 
        Exploded 
    }
    public UnityEvent OnFuseStarted;

    private const int _maxBlastHits = 64;

    private static readonly Collider[] _blastHits = new Collider[_maxBlastHits];
    private static readonly Dictionary<IDamageable, float> _blastDamage = new();

    [Header("Behaviour")]
    [SerializeField] private bool _keepChasingWhenOutOfRange = true;

    [Header("Fuse")]
    [SerializeField] private float _triggerDistance = 2f;
    [SerializeField] private float _fuseTime = 0.5f;
    [SerializeField] private float _chainReactionDelay = 0.1f;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionDamage = 50f;
    [SerializeField] private float _edgeDamageMultiplier = 0.25f;
    [SerializeField] private float _explosionHeight = 1f;
    [SerializeField] private LayerMask _damageMask = ~0;
    [SerializeField] private LayerMask _blastBlockingMask = ~0;
    [SerializeField] private GameObject _explosionEffect;

    private State _state = State.Idle;
    private float _explodeTime;

    private Vector3 ExplosionOrigin => transform.position + Vector3.up * _explosionHeight;

    protected override void Awake()
    {
        base.Awake();

        if (Agent.stoppingDistance >= _triggerDistance)
        {
            Debug.LogWarning($"{name}: stopping distance must be smaller than TriggerDistance. Clamping it.", this);
            Agent.stoppingDistance = _triggerDistance * 0.5f;
        }

        EnemyType = "67 Guy";
    }

    protected override void Reset()
    {
        base.Reset();

        _state = State.Idle;
    }

    protected override void UpdateBehavior()
    {
        if (_state == State.Idle && TargetDetected) _state = State.Chasing;
        if (_state == State.Chasing) HandleChasing();
    }

    public void Alert()
    {
        if (_state == State.Idle) _state = State.Chasing;
    }

    // Idealmente tem que ser chamada na morte do inimigo
    public void Detonate()
    {
        if (_state != State.Exploded) StartFuse(_chainReactionDelay, notify: false);
    }

    // ---------- States ----------
    private void HandleChasing()
    {
        if (!TargetDetected && !_keepChasingWhenOutOfRange)
        {
            _state = State.Idle;
            Halt();
            return;
        }

        if (TargetDistance <= _triggerDistance)
        {
            StartFuse(_fuseTime, notify: true);
            return;
        }

        MoveToPlayer();
        FaceDirection(Agent.desiredVelocity);
    }

    private void StartFuse(float duration, bool notify)
    {
        float newExplodeTime = Time.time + duration;

        if (_state == State.Fusing)
        {
            _explodeTime = Mathf.Min(_explodeTime, newExplodeTime);
            return;
        }

        _state = State.Fusing;
        _explodeTime = newExplodeTime;
        Halt();

        if (notify) OnFuseStarted?.Invoke();

        StartCoroutine(FuseRoutine());
    }

    private IEnumerator FuseRoutine()
    {
        do
        {
            FacePlayer();
            yield return null;
        }
        while (Time.time < _explodeTime);

        Explode();
    }

    // ---------- Explosion ----------
    private void Explode()
    {
        _state = State.Exploded;

        Vector3 origin = ExplosionOrigin;

        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, origin, Quaternion.identity);
        }

        int hitCount = Physics.OverlapSphereNonAlloc(origin, _explosionRadius, _blastHits, _damageMask, QueryTriggerInteraction.Collide);

        _blastDamage.Clear();

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = _blastHits[i];
            _blastHits[i] = null;

            if (hit.transform.IsChildOf(transform)) continue;

            IDamageable target = hit.GetComponent<IDamageable>();
            if (target == null) continue;

            Vector3 closestPoint = GetClosestPoint(hit, origin);
            if (IsBlastBlocked(origin, closestPoint)) continue;

            float damage = CalculateDamage(Vector3.Distance(origin, closestPoint));

            if (!_blastDamage.TryGetValue(target, out float current) || damage > current)
            {
                _blastDamage[target] = damage;
            }
        }

        foreach (KeyValuePair<IDamageable, float> entry in _blastDamage)
        {
            if (entry.Key != null)
            {
                entry.Key.TakeDamage(entry.Value, transform.position, 0);
            }
        }

        _blastDamage.Clear();
        Die();
    }

    private float CalculateDamage(float distance)
    {
        float t = Mathf.Clamp01(distance / _explosionRadius);
        return _explosionDamage * Mathf.Lerp(1f, _edgeDamageMultiplier, t);
    }

    private static Vector3 GetClosestPoint(Collider collider, Vector3 point)
    {
        if (collider is MeshCollider mesh && !mesh.convex)
        {
            return collider.bounds.ClosestPoint(point);
        }

        return collider.ClosestPoint(point);
    }

    private bool IsBlastBlocked(Vector3 origin, Vector3 point)
    {
        if (!Physics.Linecast(origin, point, out RaycastHit hit, _blastBlockingMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return hit.collider.GetComponentInParent<Damageable>() == null && !hit.transform.IsChildOf(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(ExplosionOrigin, _explosionRadius);
    }
}