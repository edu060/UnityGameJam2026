using UnityEngine;

public class CharacterAudioHandler : MonoBehaviour
{
    [Header("AudioFiles")]
    [SerializeField] private CharacterAudiosSO _audios;
    [Header("Components")]
    [SerializeField] private MovementController _movement;
    [SerializeField] private Gun _gun;

    private void SubscribeToEvents()
    {
        
    }


    private void Shoot()
    {
        _audios.PickRandom(_audios.Attack);
    }
    
}
