using UnityEngine;

public class CharacterGraphicsController : MonoBehaviour
{
    [SerializeField] private CharacterAudioHandler _audioHandler;

    private void Start()
    {
        _audioHandler.GetComponent<CharacterAudioHandler>();

        _audioHandler.enabled = true;
    }
}
