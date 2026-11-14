using UnityEngine;

public class Imp : Enemy
{
    private enum State { Patrolling, Waiting, Chasing, Attacking }

    [Header("Patrol")]
    [SerializeField] private float _patrolRadius = 8f;
    [SerializeField] private float _waitTime = 2f;

    [Header("Combat")]
    [SerializeField] private NpcGun _gun;
    [SerializeField] private float _attackRange = 8f;
    [SerializeField] private float _attackCooldown = 2f;

    //[Header("Projectile")]
    //[SerializeField] private Transform _projectileSpawn;
    //[SerializeField] private GameObject _projectile;

    private State _state = State.Patrolling;
    private Vector3 _patrolCenter;
    private float _waitTimer;
    private float _nextAttackTime;

    private bool InCombat => _state == State.Chasing || _state == State.Attacking;

    private void Start()
    {
        _patrolCenter = transform.position;
        ChoosePatrolDestination();
    }

    protected override void Awake()
    {
        base.Awake();

        EnemyType = "Looksmaxxer";
    }

    protected override void Reset()
    {
        base.Reset();

        _state = State.Patrolling;
    }

    protected override void UpdateBehavior()
    {
        if (TargetDetected && !InCombat)
        {
            _state = State.Chasing;
            SetMoving(true);
        }
        else if (!TargetDetected && InCombat)
        {
            ChoosePatrolDestination();
        }

        switch (_state)
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
            _state = State.Waiting;
            _waitTimer = _waitTime;
            SetMoving(false);
        }
    }

    private void HandleWaiting()
    {
        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0f) ChoosePatrolDestination();
    }

    private void HandleChasing()
    {
        if (TargetDistance <= _attackRange)
        {
            _state = State.Attacking;
            SetMoving(false);
            return;
        }

        MoveToPlayer();
        FacePlayer();
    }

    private void HandleAttacking()
    {
        if (TargetDistance > _attackRange)
        {
            _state = State.Chasing;
            SetMoving(true);
            return;
        }

        FacePlayer();

        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackCooldown;
            Attack();
        }
    }

    private void ChoosePatrolDestination()
    {
        if (RandomNavMeshPosition(_patrolCenter, _patrolRadius, out Vector3 destination))
        {
            SetDestination(destination);
            SetMoving(true);
            _state = State.Patrolling;
        }
        else
        {
            _state = State.Waiting;
            _waitTimer = _waitTime;
        }
    }

    private void Attack()
    {
        _gun.RequestShoot();

        //if (_projectile == null || _projectileSpawn == null) return;

        //Vector3 origin = _projectileSpawn.position;
        //Quaternion aim = Quaternion.LookRotation(Target.position - origin);
        //Instantiate(_projectile, origin, aim);
    }
}