using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;

    public void EndShoot()
    {
        _gun.HandleCanShoot();
    }

    public void EndReload()
    {
        _gun.Reloaded();
    }

    public void ReloadOneBullet(bool isLastBullet = false)
    {
        _gun.AddOneBullet();
    }


    private void SubscribeToEvents()
    {
        _gun.OnShoot += PlayShootAnimation;
        _gun.OnStartReload += PlayReloadAnimation;
    }
    
    private void UnSubscribeToEvents()
    {
        _gun.OnShoot -= PlayShootAnimation;
        _gun.OnStartReload -= PlayReloadAnimation;
    }


    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void PlayShootAnimation()
    {
        _animator.SetTrigger("Shoot");
    }

    private void PlayReloadAnimation()
    {
        _animator.SetTrigger("Reload");
    }
}
