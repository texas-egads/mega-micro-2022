using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ? _instance : FindObjectOfType<GameManager>();
    public enum GameState
    {
        TitleScreen, MainGame, EndScreen
    }
    
    public GameState gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            switch (value)
            {
                case GameState.TitleScreen:
                    titleScreenListener.Invoke();
                    break;
                case GameState.MainGame:
                    mainGameListener.Invoke();
                    break;
                case GameState.EndScreen:
                    endScreenListener.Invoke();
                    break;
            }
        }
    }

    public UnityEvent titleScreenListener = new UnityEvent();
    public UnityEvent mainGameListener = new UnityEvent();
    public UnityEvent endScreenListener = new UnityEvent();
    
    private GameState _gameState;

    [SerializeField] private int roundsToWin;
    //public event Action OnMainGameStart;
    


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        //SceneManager.sceneUnloaded += StartGame<SceneManager.GetSceneByName("Title Screen")>;
        //if(SceneManager.GetActiveScene().name == "Main") StartGame();
        //TitleScreenListener.AddListener(StartGame);
    }

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen": gameState = GameState.TitleScreen;
                break;
            case "Main": gameState = GameState.MainGame;
                break;
            case "End": gameState = GameState.EndScreen;
                break;
        }
    }

    public void LoadScene(string scene)
    {
        switch (scene)
        {
            case "TitleScreen":
                gameState = GameState.TitleScreen;
                break;
            case "Main":
                gameState = GameState.MainGame;
                break;
            case "End":
                gameState = GameState.EndScreen;
                break;
            default:
                print("Wrong scene name buddy: " + scene);
                break;
        }
        SceneManager.LoadScene(scene);
    }
}
