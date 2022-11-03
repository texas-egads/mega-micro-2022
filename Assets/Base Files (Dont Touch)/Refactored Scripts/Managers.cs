using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{    
    private static Managers _instance;

    /// <summary>
    /// Jammers shouldn't have any reason to access Managers.instance. Everything accessible
    /// from here should be considered internal and not be used during minigames.
    /// </summary>
    public static Managers instance => _instance;

    [SerializeField] private MinigamesManager _minigamesManager;
    public MinigamesManager minigamesManager => _minigamesManager;
    public static IMinigamesManager MinigamesManager => instance.minigamesManager;

    [SerializeField] private AudioManager _audioManager;
    public AudioManager audioManager => _audioManager;
    public static IMinigameAudioManager AudioManager => instance.audioManager;

    [SerializeField] private ScenesManager _scenesManager;
    public ScenesManager scenesManager => scenesManager;

    private void Awake() {
        if (instance == null) {
            // we are the chosen one
            _instance = this;

            minigamesManager.Initialize();
            audioManager.Initialize();
            scenesManager.Initialize();

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            // goodbye
            Destroy(gameObject);
        }
    }
}
