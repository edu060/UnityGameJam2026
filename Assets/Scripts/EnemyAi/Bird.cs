using UnityEngine;
using UnityEngine.AI;

public class Bird : Enemy
{
    private enum State { 
        Roaming, 
        Chasing, 
        Windup, 
        Charging, 
        Recovering 
    }

    private static readonly Collider[] HitBuffer = new Collider[16];

    [Header("Roaming")]
    [SerializeField] private float roamRadius = 6f;
    [SerializeField] private float roamWaitTime = 1f;

    [Header("Charge")]
    [SerializeField] private float chargeRange = 10f;
    [SerializeField] private float windupTime = 0.4f;
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float minChargeInterval = 2f;
    [SerializeField] private float maxChargeInterval = 4f;
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private float recoveryTime = 1f;

    [Header("Bash")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float hitRadius = 0.8f;
    [SerializeField] private LayerMask damageMask = ~0;

    private State state = State.Roaming;
    private Vector3 roamCenter;
    private Vector3 chargeDirection;
    private float stateTimer;
    private float nextChargeTime;

    private void Start()
    {
        roamCenter = Agent.nextPosition;
        ChooseRoamDestination();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Reset()
    {
        base.Reset();

        state = State.Roaming;
    }

    protected override void UpdateBehavior()
    {
        switch (state)
        {
            case State.Roaming: HandleRoaming(); break;
            case State.Chasing: HandleChasing(); break;
            case State.Windup: HandleWindup(); break;
            case State.Charging: HandleCharging(); break;
            case State.Recovering: HandleRecovering(); break;
        }
    }

    // ---------- States ----------
    private void HandleRoaming()
    {
        if (TargetDetected)
        {
            BeginChase();
            return;
        }

        FaceDirection(Agent.desiredVelocity);

        if (HasReachedDestination()) StartRecovering(roamWaitTime);
    }

    private void HandleChasing()
    {
        if (!TargetDetected)
        {
            ChooseRoamDestination();
            return;
        }

        FacePlayer();
        MoveToPlayer();

        if (Time.time >= nextChargeTime && TargetDistance <= chargeRange)
        {
            state = State.Windup;
            stateTimer = windupTime;
            Halt();
        }
    }

    private void HandleWindup()
    {
        FacePlayer();

        stateTimer -= Time.deltaTime;
        if (stateTimer > 0f) return;

        Vector3 toPlayer = Target.position - transform.position;
        toPlayer.y = 0f;

        chargeDirection = toPlayer.sqrMagnitude > 0.001f ? toPlayer.normalized : transform.forward;
        stateTimer = maxChargeTime;
        state = State.Charging;
    }

    private void HandleCharging()
    {
        stateTimer -= Time.deltaTime;

        Vector3 step = chargeDirection * (chargeSpeed * Time.deltaTime);

        bool blocked = !AgentReady || NavMesh.Raycast(Agent.nextPosition, Agent.nextPosition + step, out NavMeshHit _, Agent.areaMask);

        if (TryHitTarget() || blocked || stateTimer <= 0f)
        {
            StartRecovering(recoveryTime);
            return;
        }

        Agent.Move(step);
        FaceDirection(chargeDirection);
    }

    private void HandleRecovering()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer > 0f) return;

        if (TargetDetected) BeginChase();
        else ChooseRoamDestination();
    }

    // ---------- Chasing ----------
    private void BeginChase()
    {
        state = State.Chasing;
        nextChargeTime = Time.time + Random.Range(minChargeInterval, maxChargeInterval);
    }

    private void StartRecovering(float duration)
    {
        state = State.Recovering;
        stateTimer = duration;
        Halt();
    }

    private void ChooseRoamDestination()
    {
        if (RandomNavMeshPosition(roamCenter, roamRadius, out Vector3 destination))
        {
            SetDestination(destination);
            state = State.Roaming;
        }
        else
        {
            StartRecovering(roamWaitTime);
        }
    }

    private bool TryHitTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, hitRadius, HitBuffer, damageMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider hit = HitBuffer[i];
            if (hit.transform.IsChildOf(transform)) continue;

            Damageable target = hit.GetComponentInParent<Damageable>();
            if (target == null) continue;

            target.TakeDamage(damage, gameObject);
            return true;
        }

        return false;
    }
}