using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{    
    private static Managers instance;

    [SerializeField] private MinigamesManager minigamesManager;
    public static IMinigamesManager MinigamesManager => instance.minigamesManager;

    [SerializeField] private AudioManager audioManager;
    public static IMinigameAudioManager AudioManager => instance.audioManager;


    private void Awake() {
        if (instance == null) {
            // we are the chosen one
            instance = this;

            minigamesManager.Initialize();
            audioManager.Initialize();

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            // goodbye
            Destroy(gameObject);
        }
    }
}
