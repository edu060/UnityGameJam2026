using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IPoolable
{
    [Header("Components")]
    [SerializeField] private HealthController _healthController;

    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("Perception")]
    [SerializeField, Min(0f)] private float _detectionRange = 15f;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float _repathInterval = 0.2f;
    [SerializeField, Min(0f)] private float _turnSpeed = 360f;

    protected NavMeshAgent Agent { get; private set; }
    protected Transform Target => _target;

    private float _nextRepathTime;

    protected float TargetDistance { get; private set; } = float.PositiveInfinity;
    protected bool TargetDetected => TargetDistance <= _detectionRange;
    protected bool AgentReady => Agent.isActiveAndEnabled && Agent.isOnNavMesh;


    public string EnemyType { get; protected set; }

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;

        if (_target == null && !string.IsNullOrEmpty("Player"))
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) _target = found.transform;
        }

        if(_healthController == null)
        {
            _healthController.GetComponent<HealthController>();
        }
        
    }

    protected virtual void Reset()
    {
        //Halt();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
        Reset();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    protected virtual void Update()
    {
        if (_target == null)
        {
            TargetDistance = float.PositiveInfinity;
            return;
        }

        TargetDistance = Vector3.Distance(transform.position, _target.position);
        UpdateBehavior();
    }

    public void OnSpawned()
    {
        Debug.Log("Spawned");
        _healthController.ResetHealth();
    }

    public void OnDespawned()
    {
        Debug.Log("Despawned");

    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    protected virtual void SubscribeToEvents()
    {
        _healthController.OnDie += Die;
    }

    protected virtual void UnSubscribeToEvents()
    {
        _healthController.OnDie -= Die;
    }
    protected void Die()
    {
        ObjectPoolManager.Instance.Despawn(gameObject);
    }

    protected abstract void UpdateBehavior();

    // ---------- Movement ----------
    protected void MoveTo(Vector3 destination)
    {
        if (Time.time >= _nextRepathTime) SetDestination(destination);
    }

    protected void SetDestination(Vector3 destination)
    {
        if (!AgentReady) return;

        _nextRepathTime = Time.time + _repathInterval;
        Agent.SetDestination(destination);
    }

    protected void MoveToPlayer()
    {
        if (_target != null) MoveTo(_target.position);
    }

    protected void SetMoving(bool moving)
    {
        if (AgentReady) Agent.isStopped = !moving;
    }

    protected bool HasReachedDestination(float tolerance = 0.5f)
    {
        return AgentReady && !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance + tolerance;
    }

    protected void Halt()
    {
        if (!AgentReady) return;

        Agent.ResetPath();
        Agent.velocity = Vector3.zero;
    }

    // ---------- Rotation ----------
    protected void FacePlayer()
    {
        if (_target != null) FaceDirection(_target.position - transform.position);
    }

    protected void FaceDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, _turnSpeed * Time.deltaTime);
    }

    // ---------- Utilities ----------
    protected bool RandomNavMeshPosition(Vector3 center, float radius, out Vector3 result, float maxSampleDistance = 2f)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 p = Random.insideUnitCircle * radius;
            Vector3 candidate = center + new Vector3(p.x, 0f, p.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, maxSampleDistance, Agent.areaMask))
            {
                result = hit.position;
                return true;
            }
        }

        result = transform.position;
        return false;
    }
}