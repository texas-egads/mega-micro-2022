using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class ScenesManagerSimple : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject creditsPage;
    public GameObject difficultyPage;

    public void Update() {
        //if user presses escape and is on credits page, go back to main page
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(creditsPage.activeSelf) {
                LoadMain();
            }
        }
    }
    //Created to load main game scenes, without risk of messing up ScenesManager.cs
    public static void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void LoadGameWithDifficulty(string difficulty) {
        PlayerPrefs.SetString("difficulty", difficulty);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    //quit game
    public static void QuitGame() {
        Application.Quit();
    }

    public void LoadCredits() {
        creditsPage.SetActive(true);
        mainPage.SetActive(false);
        if(difficultyPage != null) {
            difficultyPage.SetActive(false);
        }
    }

    public void LoadMain() {
        creditsPage.SetActive(false);
        mainPage.SetActive(true);
        if(difficultyPage != null) {
            difficultyPage.SetActive(false);
        }
    }

    public void LoadDifficulty() {
        if(difficultyPage != null) {
            difficultyPage.SetActive(true);
        }
        mainPage.SetActive(false);
        creditsPage.SetActive(false);
    }
}
