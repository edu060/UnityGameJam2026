using UnityEngine;
using UnityEngine.Events;

public class NpcGun : Gun
{
    public UnityAction OnEndShoot;
    public UnityAction OnRequestShoot;

    public void RequestShoot()
    {
        Debug.Log("RequestShoot");
        Shoot(false,false,false);
        OnRequestShoot?.Invoke();
    }
}
