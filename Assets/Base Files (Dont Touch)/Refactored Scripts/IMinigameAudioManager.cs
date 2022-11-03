using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigameAudioManager
{
    /// <summary>
    /// Fetches a managed AudioSource component from the manager. This component is attached to a
    /// game object that exists in the DontDestroyOnLoad scene. The AudioSource and its GameObject
    /// should (hopefully) have all of their parameters set to default. To play a sound, you should
    /// call this to obtain an AudioSource, assign it a clip, change any audio parameters, and then
    /// play it. Do not create your own audio sources and play those. Do not change the mixer
    /// group that the audio source outputs into.
    /// </summary>
    /// <returns>An AudioSource instance, or null if too many AudioSources are in use.</returns>
    AudioSource GetAudioSource();

    /// <summary>
    /// Flags an AudioSource to automatically be returned to the manager (see ReturnAudioSource)
    /// when it finishes playing. This is useful for one-shot sound effects. If the audio source
    /// is not playing or is looping, this method will not do anything.
    /// </summary>
    /// <param name="source">The managed audio source that you want to return when it finishes.</param>
    void ReturnAudioSourceWhenFinished(AudioSource source);

    /// <summary>
    /// Gives an AudioSource back to the manager for reuse. Call this whenever you're done with
    /// an AudioSource that you previously obtained by using GetAudioSource. The manager will
    /// stop the audio and add it back to its pool to be used again by someone else. ReturnAudioSource
    /// will automatically run for any AudioSource that has not been returned when the minigame is
    /// over.
    /// </summary>
    /// <param name="source">The managed audio source you want to return.</param>
    void ReturnAudioSource(AudioSource source);
}
