using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private string mainSceneName;
    
    public void Initialize() {
        Scene startingScene = SceneManager.GetActiveScene();
        
        if (startingScene.name == mainSceneName) {
            // TODO maybe do something here   
            
            return;
        }

        MinigameDefinition def = Managers.instance.minigamesManager.GetMinigameDefForScene(startingScene);
        if (def != null) {
            Managers.instance.minigamesManager.AddMinigameToList(def);
            LoadSceneImmediate(mainSceneName);
        }
    }


    public void LoadSceneImmediate(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
