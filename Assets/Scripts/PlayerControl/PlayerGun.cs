using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : Gun
{
    [Header("Effects Values")]
    [SerializeField] private List<Transform> _extraBulletsPoints;
    [SerializeField] private float _slowBulletSpeed = 2f;
    [SerializeField] private float _bigBulletSize = 2f;
    
    private void OnEnable()
    {
        SubscribeToEvents();
        Initialize();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Attack.performed += DoShoot;
        OnEndReload += ActiveRandomEffect;
    }

    private void UnSubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Attack.performed -= DoShoot;
        OnEndReload -= ActiveRandomEffect;
    }

    private void ActiveRandomEffect()
    {
        int randomEffect = Random.Range(0, 5);

        switch(randomEffect)
        {
            case 0:
                ActiveEffects.BiggerBullets = !ActiveEffects.BiggerBullets;
                break;
            case 1:
                ActiveEffects.SlowBullets = !ActiveEffects.SlowBullets;
                break;
            case 2:
                ActiveEffects.ExplosiveBullets = !ActiveEffects.ExplosiveBullets;
                break;
            case 3:
                ActiveEffects.MultipleBullets = !ActiveEffects.MultipleBullets;
                break;
            case 4:
                ActiveEffects.RicochetableBullets = !ActiveEffects.RicochetableBullets;
                break;

        }
    }

    private void DoShoot(InputAction.CallbackContext context)
    {
        if(ActiveEffects.MultipleBullets)
        {
            foreach(Transform t in _extraBulletsPoints)
            {
                PlayerShoot(t, false, false, false);
            }
        }

        PlayerShoot(ShootingPoint);
    }

    private void PlayerShoot(Transform point, bool costAmmo = true, bool useCanShoot = true, bool invokeEvent = true)
    {
        Bullet bullet = Shoot(costAmmo, useCanShoot, invokeEvent);
        if(bullet != null)
        {
            bullet.transform.SetPositionAndRotation(point.position, point.rotation);

            bullet.SetRicochetable(ActiveEffects.RicochetableBullets);
            bullet.SetExplosive(ActiveEffects.ExplosiveBullets);

            if (ActiveEffects.BiggerBullets)
            {
                bullet.transform.localScale *= _bigBulletSize;
            }
            if (ActiveEffects.SlowBullets)
            {
                bullet.SetSpeed(_slowBulletSpeed);
            }
        }
    }
}
