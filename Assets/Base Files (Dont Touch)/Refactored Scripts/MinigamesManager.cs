using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour, IMinigamesManager
{
    public const int STARTING_LIVES = 3;

    [SerializeField] private List<MinigameDefinition> allMinigames;
    [SerializeField] private int numRoundsInEasyMode;
    [SerializeField] private int numRoundsInNormalMode;

    public Action<MinigameStatus> OnBeginIntermission;
    public Action OnStartMinigame;
    public Action OnEndMinigame;

    private MinigameStatus status;
    private List<MinigameDefinition> minigames;

    private bool isMinigamePlaying;
    private bool isCurrentMinigameWon;

    // stores a preloaded scene for an upcoming round.
    private AsyncOperation nextMinigameLoadOperation;
    private Scene nextMinigameScene;

    public void Initialize() {
        minigames = new List<MinigameDefinition>();
        isMinigamePlaying = false;
        isCurrentMinigameWon = false;
    }

    public void PopulateMinigameList(int numberOfRounds) {
        minigames.Clear();

        // right now we assume that there's only one miniboss and boss. you'll want to change this if that's not true
        MinigameDefinition boss = null;
        MinigameDefinition miniboss = null;
        List<MinigameDefinition> normalMinigames = new List<MinigameDefinition>();
        foreach (MinigameDefinition def in minigames) {
            if (def.minigameType == MinigameType.Boss)
                boss = def;
            else if (def.minigameType == MinigameType.Miniboss)
                miniboss = def;
            else
                normalMinigames.Add(def);
        }

        // bag randomize the normal minigames
        normalMinigames.Shuffle();

        for (int i = 0; i < numberOfRounds; i++) {
            if (i == (numberOfRounds - 1) / 2 && miniboss != null) {
                AddMinigameToList(miniboss);
            }
            else if (i == numberOfRounds - 1 && boss != null) {
                AddMinigameToList(boss);
            }
            else {
                AddMinigameToList(normalMinigames.Last());
                normalMinigames.RemoveAt(normalMinigames.Count - 1);
            }
        }
    }

    public void AddMinigameToList(MinigameDefinition def) {
        minigames.Add(def);
    }

    public void StartMinigames() {
        if (minigames.Count == 0) {
            PopulateMinigameList(numRoundsInNormalMode);
        }

        status.currentHealth = STARTING_LIVES;
        status.nextRoundNumber = 0;

        RunIntermission(status);
    }


    public MinigameDefinition GetCurrentMinigameDefinition() {
        if (status.nextRoundNumber < 0 || status.nextRoundNumber >= minigames.Count)
            return null;
        
        return minigames[status.nextRoundNumber];
    }
    
    public void DeclareCurrentMinigameWon() {
        if (!isMinigamePlaying)
            return;
        isCurrentMinigameWon = true;
    }

    public void DeclareCurrentMinigameLost() {
        if (!isMinigamePlaying)
            return;
        isCurrentMinigameWon = false;
    }

    public void EndCurrentMinigame() {
        if (!isMinigamePlaying)
            return;
        
        isMinigamePlaying = false;
        OnEndMinigame?.Invoke();

        status.previousMinigame = GetCurrentMinigameDefinition();

        if (isCurrentMinigameWon) {
            status.nextRoundNumber = status.nextRoundNumber + 1;

            if (status.previousMinigame.minigameType != MinigameType.Normal)
                status.healthDelta = 1;
        }
        else {
            status.healthDelta = -1;
        }

        status.currentHealth += status.healthDelta;
        status.hasWonGame = status.nextRoundNumber >= minigames.Count;

        if (!status.hasLostGame && !status.hasWonGame) {
            status.nextMinigame = GetCurrentMinigameDefinition();
            LoadMinigame(status.nextMinigame);
        }

        RunIntermission(status);
    }


    public void RunIntermission(MinigameStatus status) {
        if (OnBeginIntermission == null) {
            Debug.LogWarning("No one is subscribed to OnBeginIntermission. This is probably a mistake because we expect a listener here to then later call LoadNextMinigame");
        }

        // TODO connect something to this
        OnBeginIntermission?.Invoke(status);
    }


    private void LoadMinigame(MinigameDefinition minigameDef) {
        StartCoroutine(DoLoadMinigame(minigameDef));
    }

    private IEnumerator DoLoadMinigame(MinigameDefinition minigameDef) {
        nextMinigameLoadOperation = SceneManager.LoadSceneAsync(minigameDef.scene.name, LoadSceneMode.Additive);
        nextMinigameLoadOperation.allowSceneActivation = false;
        yield break;
    }

    // Called when all of the between-minigame cinematics are complete and the
    // next minigame is ready to be put on screen.
    public void StartNextMinigame() {
        if (isMinigamePlaying) {
            Debug.LogError("Cannot load next minigame when a minigame is playing!");
            return;
        }

        StartCoroutine(DoStartNextMinigame());
    }

    private IEnumerator DoStartNextMinigame() {
        // wait for the minigame scene to load
        while (nextMinigameLoadOperation.progress < 0.9f)
            yield return null;
        
        nextMinigameLoadOperation.allowSceneActivation = true;
        while (!nextMinigameLoadOperation.isDone)
            yield return null;

        isCurrentMinigameWon = false;
        isMinigamePlaying = true;

        OnStartMinigame?.Invoke();

        // TODO activate it and switch over control

        yield break;
    }


    public MinigameDefinition GetMinigameDefForScene(Scene scene) {
        return minigames.Find(mDef => mDef.scene == scene);
    }
}
