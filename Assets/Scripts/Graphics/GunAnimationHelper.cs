using UnityEngine;

public class GunAnimationHelper : MonoBehaviour
{
    [SerializeField] private GunAnimation _gunAnimation;

    public void EndShoot()
    {
        _gunAnimation.EndShoot();
    }

    public void EndReload()
    {
        _gunAnimation.EndReload();
    }

    public void ReloadBullet()
    {
        _gunAnimation.ReloadOneBullet();

        // Play PlaceBullet Sound
    }

    public void ReloadLastBullet()
    {
        _gunAnimation.ReloadOneBullet(true);

        // Play LastBullet Sound
    }

    public void SpinTambor()
    {
        // Play SpinTambor sound
    }
}
