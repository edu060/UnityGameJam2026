using UnityEngine;

public class GunAnimationHelper : MonoBehaviour
{
    [SerializeField] private GunAnimation _gunAnimation;
    [SerializeField] private CharacterAudioHandler _characterAudioHandler;

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

        _characterAudioHandler.ReloadOneBullet();
        // Play PlaceBullet Sound
    }

    public void ReloadLastBullet()
    {
        _gunAnimation.ReloadOneBullet(true);

        _characterAudioHandler.ReloadLastBullet();
        // Play LastBullet Sound
    }

    public void SpinTambor()
    {
        // Play SpinTambor sound
    }
}
