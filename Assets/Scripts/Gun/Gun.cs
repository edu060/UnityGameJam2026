using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityAction OnShoot;
    public UnityAction OnEndReload;
    public UnityAction OnStartReload;
    public UnityAction OnAmmoUpdate;

    [SerializeField] private Collider _ownCollider;

    [Space]
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootingSpacing;
    [SerializeField] private Transform _shootingPoint;

    [SerializeField] private int _maxAmmo;

    protected string BulletName;
    
    private int _currentAmmo;
    private bool _canShoot = true;
    private bool _needReload = false;

    protected Transform ShootingPoint => _shootingPoint;

    public int MaxAmmo => _maxAmmo;
    public int CurrentAmmo
    {
        get {  return _currentAmmo; }
        private set
        {
            _currentAmmo = value;
            OnAmmoUpdate?.Invoke();
        }
    }

    private void OnEnable()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        CurrentAmmo = _maxAmmo;
        _canShoot = true;
        BulletName = PoolsNames.Bullet;
    }

    public void HandleCanShoot()
    {
        if (CurrentAmmo <= 0)
        {
            _canShoot = false;
            _needReload = true;
            StartReaload();
            return;
        }

        _canShoot = true;
        _needReload = false;
    }

    public void AddOneBullet()
    {
        CurrentAmmo++;
    }

    public void Reloaded()
    {
        CurrentAmmo = _maxAmmo;

        _needReload = false;
        _canShoot = true;

        OnEndReload?.Invoke();
    }

    protected Bullet Shoot(bool costAmmo = true, bool useCanShoot = true, bool invokeEvent = true)
    {
        if (!_canShoot || (costAmmo && CurrentAmmo <= 0))
            return null;

        GameObject obj = ObjectPoolManager.Instance.Spawn(BulletName, _shootingPoint.position, _shootingPoint.rotation);
        if (obj != null)
        {
            Bullet newBullet = obj.GetComponent<Bullet>();
            
            newBullet.SetSpeed(_bulletSpeed);
            newBullet.SetOwner(_ownCollider);

            if(costAmmo)
            {
                CurrentAmmo--;
            }

            if(useCanShoot)
            {
                _canShoot = false;
            }

            if (invokeEvent)
            {
                OnShoot?.Invoke();
            }

            return newBullet;
        }

        return null;
    }

    protected void StartReaload()
    {
        OnStartReload?.Invoke();
    }
}
