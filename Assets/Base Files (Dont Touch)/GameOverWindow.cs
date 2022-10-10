using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;

    private void Awake()
    {
        window.SetActive(false);
        MainGameManager.Instance.GameOver += GameLose;
    }

    private void GameLose()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        window.SetActive(false);
        MainGameManager.Instance.RestartGame();
    }

    public void TitleButton()
    {
        window.SetActive(false);
        GameManager.Instance.LoadScene("TitleScreen");
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.GameOver -= GameLose;
    }
}
