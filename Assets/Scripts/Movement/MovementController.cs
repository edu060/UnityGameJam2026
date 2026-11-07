using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 1f;
    protected float VelocityMagTolerance = 0.2f;

    public Vector3 Direction { get; protected set; }
    public float VelocityMag { get; private set; }

    public bool CanMove {get; protected set; }

    public float MovementSpeed => _movementSpeed;

    public void ResetMovement()
    {
        CanMove = false;
    }
    public virtual void Setup()
    {
        CanMove = true;
    }

    protected virtual void Update()
    {
        DoMovement();
    }

    protected void StopMovement()
    {
        Direction = Vector3.zero;
        VelocityMag = 0f;
    }

    protected void SetVelocity(float velocity)
    {
        _movementSpeed = velocity;
    }

    protected abstract void DoMovement();

}
