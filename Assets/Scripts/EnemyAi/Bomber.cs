using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomber : Enemy
{
    private enum State { 
        Idle, 
        Chasing, 
        Fusing, 
        Exploded 
    }

    private const int MaxBlastHits = 64;

    private static readonly Collider[] BlastHits = new Collider[MaxBlastHits];
    private static readonly Dictionary<Damageable, float> BlastDamage = new();

    [Header("Behaviour")]
    [SerializeField] private bool keepChasingWhenOutOfRange = true;

    [Header("Fuse")]
    [SerializeField] private float TriggerDistance = 2f;
    [SerializeField] private float fuseTime = 0.5f;
    [SerializeField] private float chainReactionDelay = 0.1f;
    [SerializeField] private UnityEvent onFuseStarted;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 50f;
    [SerializeField] private float edgeDamageMultiplier = 0.25f;
    [SerializeField] private float explosionHeight = 1f;
    [SerializeField] private LayerMask damageMask = ~0;
    [SerializeField] private LayerMask blastBlockingMask = ~0;
    [SerializeField] private GameObject explosionEffect;

    private State state = State.Idle;
    private float explodeTime;

    private Vector3 ExplosionOrigin => transform.position + Vector3.up * explosionHeight;

    protected override void Awake()
    {
        base.Awake();

        if (Agent.stoppingDistance >= TriggerDistance)
        {
            Debug.LogWarning($"{name}: stopping distance must be smaller than TriggerDistance. Clamping it.", this);
            Agent.stoppingDistance = TriggerDistance * 0.5f;
        }
    }

    protected override void UpdateBehavior()
    {
        if (state == State.Idle && TargetDetected) state = State.Chasing;
        if (state == State.Chasing) HandleChasing();
    }

    public void Alert()
    {
        if (state == State.Idle) state = State.Chasing;
    }

    // Idealmente tem que ser chamada na morte do inimigo
    public void Detonate()
    {
        if (state != State.Exploded) StartFuse(chainReactionDelay, notify: false);
    }

    // ---------- States ----------
    private void HandleChasing()
    {
        if (!TargetDetected && !keepChasingWhenOutOfRange)
        {
            state = State.Idle;
            Halt();
            return;
        }

        if (TargetDistance <= TriggerDistance)
        {
            StartFuse(fuseTime, notify: true);
            return;
        }

        MoveToPlayer();
        FaceDirection(Agent.desiredVelocity);
    }

    private void StartFuse(float duration, bool notify)
    {
        float newExplodeTime = Time.time + duration;

        if (state == State.Fusing)
        {
            explodeTime = Mathf.Min(explodeTime, newExplodeTime);
            return;
        }

        state = State.Fusing;
        explodeTime = newExplodeTime;
        Halt();

        if (notify) onFuseStarted?.Invoke();

        StartCoroutine(FuseRoutine());
    }

    private IEnumerator FuseRoutine()
    {
        do
        {
            FacePlayer();
            yield return null;
        }
        while (Time.time < explodeTime);

        Explode();
    }

    // ---------- Explosion ----------
    private void Explode()
    {
        state = State.Exploded;

        Vector3 origin = ExplosionOrigin;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, origin, Quaternion.identity);
        }

        int hitCount = Physics.OverlapSphereNonAlloc(origin, explosionRadius, BlastHits, damageMask, QueryTriggerInteraction.Collide);

        BlastDamage.Clear();

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = BlastHits[i];
            BlastHits[i] = null;

            if (hit.transform.IsChildOf(transform)) continue;

            Damageable target = hit.GetComponentInParent<Damageable>();
            if (target == null) continue;

            Vector3 closestPoint = GetClosestPoint(hit, origin);
            if (IsBlastBlocked(origin, closestPoint)) continue;

            float damage = CalculateDamage(Vector3.Distance(origin, closestPoint));

            if (!BlastDamage.TryGetValue(target, out float current) || damage > current)
            {
                BlastDamage[target] = damage;
            }
        }

        foreach (KeyValuePair<Damageable, float> entry in BlastDamage)
        {
            if (entry.Key != null) entry.Key.TakeDamage(entry.Value, gameObject);
        }

        BlastDamage.Clear();
        Destroy(gameObject);
    }

    private float CalculateDamage(float distance)
    {
        float t = Mathf.Clamp01(distance / explosionRadius);
        return explosionDamage * Mathf.Lerp(1f, edgeDamageMultiplier, t);
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
        if (!Physics.Linecast(origin, point, out RaycastHit hit, blastBlockingMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return hit.collider.GetComponentInParent<Damageable>() == null && !hit.transform.IsChildOf(transform);
    }
}