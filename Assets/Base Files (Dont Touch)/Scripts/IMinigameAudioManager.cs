using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigameAudioManager
{
    /// <summary>
    /// Creates a game object with an audio source that exists in the DontDestroyOnLoad scene.
    /// The AudioSource and its GameObject have all of their parameters set to their defaults.
    /// 
    /// To play a sound in a minigame, you should call this to obtain an AudioSource, assign it
    /// a clip, change any audio parameters, and then play it. Do not create your own audio sources
    /// and play those. Do not change the mixer group that the audio source outputs into. Do not
    /// delete any audio sources obtained via this method.
    /// </summary>
    /// <returns>An AudioSource instance, or null if too many AudioSources are in use.</returns>
    AudioSource CreateAudioSource();
}
