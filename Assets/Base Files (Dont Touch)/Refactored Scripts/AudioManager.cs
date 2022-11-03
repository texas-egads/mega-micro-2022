using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IMinigameAudioManager
{
    [SerializeField] private int POOL_MAX_SIZE = 200;
    [SerializeField] private AudioMixerGroup minigameMixerGroup;

    public HashSet<AudioSource> occupiedAudioSources;

    public void Initialize() {
        GameObject newObj = new GameObject();
        newObj.transform.parent = transform;
        occupiedAudioSources = new HashSet<AudioSource>();
    }

    public AudioSource GetAudioSource() {
        if (occupiedAudioSources.Count >= POOL_MAX_SIZE) {
            Debug.LogError("You're asking for too many audio sources! Please reuse some old ones, or play less sounds.");
            return null;
        }

        GameObject newObj = new GameObject();
        newObj.transform.parent = transform;
        AudioSource source = newObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = minigameMixerGroup;
        occupiedAudioSources.Add(source);

        return source;
    }

    public void ReturnAudioSourceWhenFinished(AudioSource source) {
        // TODO
    }

    public void ReturnAudioSource(AudioSource source) {
        if (!occupiedAudioSources.Remove(source)) {
            Debug.LogError("You're returning an audio source that isn't currently in use!");
            return;
        }

        source.Stop();
        Destroy(source.gameObject);
    }

    public void StartMinigameAudio() {
        minigameMixerGroup.audioMixer.SetFloat("MinigameVolume", 0);
    }

    public void FadeMinigameAudio() {
        // TODO run a fade tween that sets the minigame volume to -80 db over time
        // when the fade is done, all of the audio sources get returned
    }

    

    /*
    [SerializeField] private AudioClip readyMusic;
    [SerializeField] private AudioClip successMusic;
    [SerializeField] private AudioClip failureMusic;
    [SerializeField] private AudioClip gameOverMusic;
    private float _volume = 1;
    private AudioSource _source;

    public void Initialize()
    {
        _source = gameObject.AddComponent<AudioSource>();
        //_source.clip = music[Random.Range(0, music.Length-1)];
        MainGameManager.Instance.MainStart += StartMusic;
        MainGameManager.Instance.GameOver += LoseMusic;
        _source.Play();
    }
    
    private void StartMusic(bool win)
    {
        StartCoroutine(MainMusicStart(win));
    }

    private IEnumerator MainMusicStart(bool win)
    {
        _source.clip = win ? successMusic : failureMusic;
        _source.volume = _volume;
        _source.Play();
        yield return new WaitForSeconds(MainGameManager.ShortTime / 2);
        _source.clip = readyMusic;
        _source.Play();
        yield return new WaitForSeconds(MainGameManager.ShortTime / 2);
        StartCoroutine(FadeMusic());
    }

    private void LoseMusic()
    {
        _source.clip = gameOverMusic;
        _source.volume = _volume;
        _source.Play();
    }

    private IEnumerator FadeMusic()
    {
        while (_source.volume > .1f)
        {
            _source.volume -= .05f;
            yield return new WaitForSeconds(.02f);
        }
        _source.Stop();
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.MainStart -= StartMusic;
        MainGameManager.Instance.GameOver -= LoseMusic;
    }
    */
}
