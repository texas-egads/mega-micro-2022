using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour, IMinigameAudioManager
{

    [SerializeField] private AudioMixerGroup minigameMixerGroup;
    public HashSet<AudioSource> occupiedAudioSources;

    public float MinigameVolume {
        get { 
            minigameMixerGroup.audioMixer.GetFloat("MinigameVolume", out float value);
            return value;
        }
        set {
            minigameMixerGroup.audioMixer.SetFloat("MinigameVolume", value);
        }
    }

    public void Initialize() {
        GameObject newObj = new GameObject();
        newObj.transform.parent = transform;
        occupiedAudioSources = new HashSet<AudioSource>();
    }

    public AudioSource CreateAudioSource() {
        GameObject newObj = new GameObject();
        newObj.transform.parent = transform;
        AudioSource source = newObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = minigameMixerGroup;
        occupiedAudioSources.Add(source);

        return source;
    }

    private void RemoveAudioSources() {
        foreach (AudioSource source in occupiedAudioSources) {
            source.Stop();
            source.DOKill();
            Destroy(source.gameObject);
        }

        occupiedAudioSources.Clear();
    }

    public void StartMinigameAudio() {
        minigameMixerGroup.audioMixer.SetFloat("MinigameVolume", 0);
    }

    public void FadeMinigameAudio() {
        DOTween.To(
            () => MinigameVolume,
            (value) => MinigameVolume = value,
            -80f,
            0.4f
        )
        .SetEase(Ease.InExpo)
        .OnComplete(RemoveAudioSources);
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
