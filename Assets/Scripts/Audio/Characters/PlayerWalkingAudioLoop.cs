using System.Collections;
using UnityEngine;

public class PlayerWalkingAudioLoop : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterAudiosSO _audios;
    [SerializeField] private float _stepSize = 40;

    [Space]
    [SerializeField] private float _amountMoved = 0;

    private bool _startMoving = false;

    private void Update()
    {
        _amountMoved += _playerMovement.Direction.magnitude;

        if(_startMoving == false && _playerMovement.Direction.magnitude > 0.1f) // Start moving now
        {
            DoStep();
            _startMoving = true;
        }
        else if(_amountMoved > _stepSize) // Moved enough to a new step
        {
            DoStep();
            _amountMoved = 0;
        }
        else if(_playerMovement.Direction.magnitude <= 0.1f && _startMoving) // Stop Moving
        {
            DoStep();
            _amountMoved = 0;
            _startMoving = false;
        }
    }

    private void DoStep()
    {
        SoundEmitter emitter = (ObjectPoolManager.Instance.Spawn(PoolsNames.SoundEmitter, transform.position, transform.rotation)).GetComponent<SoundEmitter>();

        if (emitter != null)
        {
            emitter.Initialize(_audios.PickRandom(_audios.FootSteps));
            emitter.Play();
        }
    }
}
