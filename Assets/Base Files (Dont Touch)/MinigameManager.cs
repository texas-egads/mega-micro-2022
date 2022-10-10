using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class MinigameManager : MonoBehaviour
{
    private static MinigameManager _instance; 
    
    //******* MINIGAME MANAGER SINGLETON *******//
    // Use this to reference your minigame Scriptable Object and the PlaySound method from other scripts
    public static MinigameManager Instance => _instance ? _instance : FindObjectOfType<MinigameManager>();
    //*****************************************//

    //******* DEBUG GAME ONLY *******//
    // set to true if you want to test your scene alone in play mode;
    [Tooltip("Set this to true if you want to test your scene without the surrounding main game.")]
    public bool debugGameOnly; 
    //****************************//

    [Tooltip("The Minigame object for your minigame! Slot it in here whenever you have deleted the example.")]
    public Minigame minigame;
    
    public void PlaySound(string soundName)
    {
        foreach (var s in minigame.sounds)
        {
            if (s.soundName == soundName)
            {
                s.source.pitch = Random.Range(s.minPitch, s.maxPitch);
                s.source.Play();
            }
        }
    }
    
    // ^^^^^^^^^ This is the only stuff you need to worry about ^^^^^^^^^^ //
    
    
    
    
    
    
    
    
    
    
    
    
    private AudioSource _musicSource;
    
    private void Awake()
    {
        _instance = this;
        minigame.gameWin = false;
        if (!debugGameOnly && GameManager.Instance == null)
        {
            debugGameOnly = true;
            SceneManager.LoadScene("Main");
            return;
        }

        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = minigame.music;
        foreach (var s in minigame.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
        if(GameManager.Instance != null )StartCoroutine(GameDelayedStart());
        else _musicSource.Play();
    }

    private IEnumerator GameDelayedStart()
    {
        yield return new WaitForSeconds(.2333f);
        MainGameManager.Instance.OnMinigameStart(minigame);
        _musicSource.Play();
    }

}

