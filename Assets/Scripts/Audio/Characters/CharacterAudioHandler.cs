using System;
using UnityEngine;

public class CharacterAudioHandler : MonoBehaviour
{
    [Header("AudioFiles")]
    [SerializeField] private CharacterAudiosSO _audios;
    [Header("Components")]
    [SerializeField] private MovementController _movement;
    [SerializeField] private Gun _gun;
    [SerializeField] private HealthController _healthController;

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
            _healthController.OnLoseHealth += TakeDamage;
            _healthController.OnGainHealth += Cure;
            _healthController.OnDie += Death;
        }

        if (_gun != null)
        {
            _gun.OnShoot += Shoot;
        }
    }

    private void UnSubscribeToEvents()
    {
        if( _healthController != null )
        {
            _healthController.OnLoseHealth -= TakeDamage;
            _healthController.OnGainHealth -= Cure;
            _healthController.OnDie -= Death;
        }

        if( _gun != null )
        {
            _gun.OnShoot -= Shoot;
        }
    }

    public void ReloadOneBullet()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.Reload));
            emitter.Play();
        }
    }

    public void ReloadLastBullet()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.LastBullet));
            emitter.Play();
        }
    }

    public void FootStep()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.FootSteps));
            emitter.Play();
        }
    }

    public void Shoot()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.Attack));
            emitter.Play();
        }
    }

    private void TakeDamage()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if( emitter != null )
        {
            emitter.Initialize(_audios.PickRandom(_audios.Hurt));
            emitter.Play();
        }
    }

    private void Cure()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.Cure));
            emitter.Play();
        }

    }

    private void Death()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.Death));
            emitter.Play();
        }
    }
    
}
