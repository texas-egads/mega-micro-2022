using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private string mainSceneName;

    private AsyncOperation currentMinigameLoad;
    private Action minigameLoadedCallback;
    bool canActivateMinigame = false;
    
    public void Initialize() {
        Scene startingScene = SceneManager.GetActiveScene();
        
        if (startingScene.name == mainSceneName) {
            // TODO maybe do something here        
            return;
        }

#if UNITY_EDITOR
        // we find the corresponding minigame definition by searching through our assets and picking it out
        // this is so that jammers don't have to modify the Managers prefab in order to add their minigame to the list
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(MinigameDefinition)}");
        MinigameDefinition def = null;
        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MinigameDefinition thisDef = AssetDatabase.LoadAssetAtPath<MinigameDefinition>(path);

            // check if the scene name is correct
            if (thisDef.sceneName == startingScene.name) {
                def = thisDef;
                break;
            }
        }
#else
        // we look through the list of minigames for the one that corresponds to ours
        MinigameDefinition def = Managers.__instance.minigamesManager.GetMinigameDefForScene(startingScene);
#endif

        if (def != null) {
            for (int i = 0; i < Managers.__instance.minigamesManager.numRoundsDebug; i++)
                Managers.__instance.minigamesManager.AddMinigameToList(def);
            
            LoadSceneImmediate(mainSceneName);
        }
    }


    public void LoadMinigameScene(MinigameDefinition minigame) {
        if (currentMinigameLoad != null) {
            Debug.LogError("Trying to load a minigame scene, but a minigame scene is already being loaded!");
            return;
        }

        StartCoroutine(DoLoadMinigame(minigame.sceneName));
    }

    public void ActivateMinigameScene(Action onLoadedCallback) {
        if (currentMinigameLoad == null) {
            Debug.LogError("Trying to activate a minigame scene, but a minigame hasn't been loaded yet!");
            return;
        }

        minigameLoadedCallback = onLoadedCallback;
        canActivateMinigame = true;
    }

    private IEnumerator DoLoadMinigame(string sceneName) {
        canActivateMinigame = false;
        currentMinigameLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentMinigameLoad.allowSceneActivation = false;

        while (currentMinigameLoad.progress < 0.9f || !canActivateMinigame)
            yield return null;
        
        currentMinigameLoad.allowSceneActivation = true;
        while (!currentMinigameLoad.isDone)
            yield return null;
        
        minigameLoadedCallback?.Invoke();
        minigameLoadedCallback = null;
        currentMinigameLoad = null;
    }


    public void LoadSceneImmediate(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
