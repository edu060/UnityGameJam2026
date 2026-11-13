using System.Collections;
using UnityEngine;

public class SoundEmitter : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioSource _audioSource;

    private Coroutine _playingCoroutine;

    public SoundData Data { get; private set; }

    public void Initialize(SoundData data)
    {
        Data = data;
        _audioSource.clip = data.Clip;
        _audioSource.outputAudioMixerGroup = data.MixerGroup;
        _audioSource.loop = data.Loop;
        _audioSource.playOnAwake = data.PlayOnAweke;
    }

    public void OnDespawned()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpawned()
    {
        throw new System.NotImplementedException();
    }

    public void Play()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }

        _audioSource.Play();
        _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    public void Stop()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }

        _audioSource.Stop();
        
        StopAllCoroutines();
        ObjectPoolManager.Instance.Despawn(gameObject);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        _audioSource.pitch += Random.Range(min, max);
    }

    private IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitWhile(() => _audioSource.isPlaying);
        ObjectPoolManager.Instance.Despawn(gameObject);
    }
}
