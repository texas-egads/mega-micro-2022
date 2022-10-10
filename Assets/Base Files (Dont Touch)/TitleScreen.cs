using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void StartGame()
    {
        GameManager.Instance.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
