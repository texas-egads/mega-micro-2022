using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundAsset
{
    [Tooltip("The name of your sound. Used with MinigameManager.Instance.PlaySound(soundName)")]
    public string soundName;

    [Tooltip("The audio data that will play for this sound.")]
    public AudioClip clip;

    [Tooltip("The volume scale of the sound.")]
    [Range(0, 1)] public float volume = 1;

    [Tooltip("Minimum possible pitch for the sound. This is here to add modulation and variety.")]
    [Range(.1f, 1)] public float minPitch = 1;

    [Tooltip("Maximum possible pitch for the sound. This is here to add modulation and variety.")]
    [Range(1, 3)] public float maxPitch = 1;

    [HideInInspector] public AudioSource source;
}
