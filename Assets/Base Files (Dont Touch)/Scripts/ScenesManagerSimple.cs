using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagerSimple : MonoBehaviour
{
    //Created to load main game scenes, without risk of messing up ScenesManager.cs
    public static void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //quit game
    public static void QuitGame() {
        Application.Quit();
    }
}
