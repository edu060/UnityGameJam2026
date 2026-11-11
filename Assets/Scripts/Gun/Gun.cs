using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityAction OnShoot;
    public UnityAction OnReload;

    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootingSpacing;
    [SerializeField] private Transform _shootingPoint;

    [SerializeField] private float _maxAmmo;

    private float _currentAmmo;

    private void OnEnable()
    {
        _currentAmmo = _maxAmmo;
    }

    protected void Shoot()
    {
        GameObject obj = ObjectPoolManager.Instance.Spawn(PoolsNames.Bullet, _shootingPoint.position, _shootingPoint.rotation);
        if (obj != null)
        {
            Bullet newBullet = obj.GetComponent<Bullet>();
            newBullet.SetSpeed(_bulletSpeed);

            _currentAmmo--;

            OnShoot?.Invoke();
        }
    }

    protected void Reload()
    {
        _currentAmmo = _maxAmmo;

        OnReload?.Invoke();
    }
}
