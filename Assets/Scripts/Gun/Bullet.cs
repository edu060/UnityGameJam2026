using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bulletSpeed;

    private void Start()
    {
        Shoot(3);
    }

    private void Update()
    {
        MoveForward();
    }

    public void Shoot(float bulletSpeed)
    {
        _bulletSpeed = bulletSpeed;
    }

    private void MoveForward()
    {
        _rigidbody.linearVelocity = new Vector3(0, 0, _bulletSpeed);
    }
}
