using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/SimpleCharacter")]
public class CharacterAudiosSO : ScriptableObject
{
    // Movement
    public List<SoundData> FootSteps;
    
    // Combat
    public List<SoundData> Attack;
    public List<SoundData> Reload;
    public List<SoundData> LastBullet;
    public List<SoundData> StartReload;
    public List<SoundData> EndReload;

    // Health
    public List<SoundData> Hurt;
    public List<SoundData> Cure;
    public List<SoundData> Death;

    public SoundData PickRandom(List<SoundData> list)
    {
        int value = (int)Random.Range(0, list.Count);
        return list[(int)Random.Range(0, list.Count)];
    }
}
