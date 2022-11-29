using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{    

    /// <summary>
    /// Jammers shouldn't have any reason to access Managers.__instance. Everything accessible
    /// from here should be considered internal and not be used during minigames.
    /// </summary>
    public static Managers __instance { get; private set; }

    [SerializeField] private MinigamesManager _minigamesManager;
    public MinigamesManager minigamesManager => _minigamesManager;
    public static IMinigamesManager MinigamesManager => __instance.minigamesManager;

    [SerializeField] private AudioManager _audioManager;
    public AudioManager audioManager => _audioManager;
    public static IMinigameAudioManager AudioManager => __instance.audioManager;

    [SerializeField] private ScenesManager _scenesManager;
    public ScenesManager scenesManager => _scenesManager;

    private void Awake() {
        if (__instance == null) {
            // we are the chosen one
            __instance = this;

            minigamesManager.Initialize();
            audioManager.Initialize();
            scenesManager.Initialize();

            DontDestroyOnLoad(gameObject);
        }
        else if (__instance != this) {
            // goodbye
            Destroy(gameObject);
        }
    }
}
