using UnityEngine;

public class SoundBuilder : MonoBehaviour
{
    private readonly SoundManager _soundManager;
    private SoundData _soundData;
    private Vector3 _position = Vector3.zero;
    private bool _randomPitch;
    private SoundEmitter _soundEmitter;

    public SoundBuilder(SoundManager soundManager)
    {
        this._soundManager = soundManager;
    }

    public SoundBuilder WithSoundData(SoundData soundData)
    {
        this._soundData = soundData;
        return this;
    }

    public SoundBuilder WithPosition(Vector3 position)
    {
        this._position = position;
        return this;
    }

    public SoundBuilder WithRandomPitch()
    {
        this._randomPitch = true;
        return this;
    }

    public void Play()
    {
        SoundEmitter soundEmitter = _soundManager.Get();
        soundEmitter.Initialize(_soundData);
        soundEmitter.transform.position = _position;
        soundEmitter.transform.parent = SoundManager.Instance.transform;

        if (_randomPitch)
        {
            soundEmitter.WithRandomPitch();
        }

        if (_soundManager.Counts.TryGetValue(_soundData, out var count))
        {
            _soundManager.Counts[_soundData] += count + 1;
        }
        else
        {
            _soundManager.Counts[_soundData] = 1;
        }

        _soundEmitter = soundEmitter;
        soundEmitter.Play();
    }

    public void Play(out SoundEmitter soundEmitter)
    {
        soundEmitter = _soundManager.Get();
        soundEmitter.Initialize(_soundData);
        soundEmitter.transform.position = _position;
        soundEmitter.transform.parent = SoundManager.Instance.transform;

        if (_randomPitch)
        {
            soundEmitter.WithRandomPitch();
        }

        if (_soundManager.Counts.TryGetValue(_soundData, out var count))
        {
            _soundManager.Counts[_soundData] += count + 1;
        }
        else
        {
            _soundManager.Counts[_soundData] = 1;
        }

        _soundEmitter = soundEmitter;
        soundEmitter.Play();
    }

}
