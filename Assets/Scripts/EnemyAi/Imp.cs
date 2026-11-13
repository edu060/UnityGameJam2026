using UnityEngine;

public class Imp : Enemy
{
    private enum State { Patrolling, Waiting, Chasing, Attacking }

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 8f;
    [SerializeField] private float waitTime = 2f;

    [Header("Combat")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Projectile")]
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private GameObject projectile;

    private State state = State.Patrolling;
    private Vector3 patrolCenter;
    private float waitTimer;
    private float nextAttackTime;

    private bool InCombat => state == State.Chasing || state == State.Attacking;

    private void Start()
    {
        patrolCenter = transform.position;
        ChoosePatrolDestination();
    }

    protected override void UpdateBehavior()
    {
        if (TargetDetected && !InCombat)
        {
            state = State.Chasing;
            SetMoving(true);
        }
        else if (!TargetDetected && InCombat)
        {
            ChoosePatrolDestination();
        }

        switch (state)
        {
            case State.Patrolling: HandlePatrolling(); break;
            case State.Waiting: HandleWaiting(); break;
            case State.Chasing: HandleChasing(); break;
            case State.Attacking: HandleAttacking(); break;
        }
    }

    private void HandlePatrolling()
    {
        FaceDirection(Agent.desiredVelocity);

        if (HasReachedDestination())
        {
            state = State.Waiting;
            waitTimer = waitTime;
            SetMoving(false);
        }
    }

    private void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f) ChoosePatrolDestination();
    }

    private void HandleChasing()
    {
        if (TargetDistance <= attackRange)
        {
            state = State.Attacking;
            SetMoving(false);
            return;
        }

        MoveToPlayer();
        FacePlayer();
    }

    private void HandleAttacking()
    {
        if (TargetDistance > attackRange)
        {
            state = State.Chasing;
            SetMoving(true);
            return;
        }

        FacePlayer();

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    private void ChoosePatrolDestination()
    {
        if (RandomNavMeshPosition(patrolCenter, patrolRadius, out Vector3 destination))
        {
            SetDestination(destination);
            SetMoving(true);
            state = State.Patrolling;
        }
        else
        {
            state = State.Waiting;
            waitTimer = waitTime;
        }
    }

    private void Attack()
    {
        if (projectile == null || projectileSpawn == null) return;

        Vector3 origin = projectileSpawn.position;
        Quaternion aim = Quaternion.LookRotation(Player.position - origin);
        Instantiate(projectile, origin, aim);
    }
}