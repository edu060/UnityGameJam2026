using UnityEngine;
using UnityEngine.U2D.Animation;

public class NpcAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NpcGun _gun;
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Animator _animator;


    [Header("Sprite Library")]
    [SerializeField] private SpriteLibrary _spriteLibrary;
    [Space]
    [SerializeField] private SpriteLibraryAsset _walkingLibrary;
    [SerializeField] private SpriteLibraryAsset _attackingLibrary;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if (_healthController != null)
        {
            _healthController.OnDie += Die;
        }

        if (_gun != null)
        {
            _gun.OnRequestShoot += RequestShoot;
        }
    }

    private void UnSubscribeToEvents()
    {
        if (_healthController != null)
        {
            _healthController.OnDie -= Die;
        }

        if (_gun != null)
        {
            _gun.OnRequestShoot -= RequestShoot;
        }
    }

    public void RequestShoot()
    {
        _spriteLibrary.spriteLibraryAsset = _attackingLibrary;
        _animator.SetTrigger("Shoot");
    }

    public void Shoot()
    {
        _gun.DoShoot();
    }

    public void EndShoot()
    {
        Debug.Log("Ended Aura Shoot");
        _spriteLibrary.spriteLibraryAsset = _walkingLibrary;
        _gun.OnEndShoot?.Invoke();
        _animator.SetTrigger("EndShoot");
    }

    private void Die()
    {

    }

}
