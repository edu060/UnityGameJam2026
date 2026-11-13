using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundData : MonoBehaviour
{
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    public bool Loop;
    public bool PlayOnAweke;
}
