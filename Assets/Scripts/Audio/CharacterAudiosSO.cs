using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/SimpleCharacter")]
public class CharacterAudiosSO : ScriptableObject
{
    public List<SoundData> FootSteps;
    public List<SoundData> Attack;

    public List<SoundData> Hurt;
    public List<SoundData> Death;

    public SoundData PickRandom(List<SoundData> list)
    {
        int value = (int)Random.Range(0, list.Count);
        return list[(int)Random.Range(0, list.Count)];
    }
}
