using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Perception")]
    [SerializeField, Min(0f)] private float detectionRange = 15f;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float repathInterval = 0.2f;
    [SerializeField, Min(0f)] private float turnSpeed = 360f;

    protected NavMeshAgent Agent { get; private set; }
    protected Transform Player => player;

    protected float TargetDistance { get; private set; } = float.PositiveInfinity;
    protected bool TargetDetected => TargetDistance <= detectionRange;
    protected bool AgentReady => Agent.isActiveAndEnabled && Agent.isOnNavMesh;

    private float nextRepathTime;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;

        if (player == null && !string.IsNullOrEmpty("Player"))
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }
    }

    protected virtual void Update()
    {
        if (player == null)
        {
            TargetDistance = float.PositiveInfinity;
            return;
        }

        TargetDistance = Vector3.Distance(transform.position, player.position);
        UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    public void SetTarget(Transform target) => player = target;

    // ---------- Movement ----------
    protected void MoveTo(Vector3 destination)
    {
        if (Time.time >= nextRepathTime) SetDestination(destination);
    }

    protected void SetDestination(Vector3 destination)
    {
        if (!AgentReady) return;

        nextRepathTime = Time.time + repathInterval;
        Agent.SetDestination(destination);
    }

    protected void MoveToPlayer()
    {
        if (player != null) MoveTo(player.position);
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
        if (player != null) FaceDirection(player.position - transform.position);
    }

    protected void FaceDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed * Time.deltaTime);
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