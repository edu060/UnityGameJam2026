using UnityEngine;
using UnityEngine.Events;

public class NpcGun : Gun
{
    public UnityAction OnEndShoot;
    public UnityAction OnRequestShoot;

    public override void Initialize()
    {
        base.Initialize();
        BulletName = PoolsNames.MoggedBullet;
    }

    public void RequestShoot()
    {
        Debug.Log("RequestShoot");
        //Shoot(false,false,false);
        OnRequestShoot?.Invoke();
    }

    public void DoShoot()
    {
        Shoot(false, false, false);
    }
}
