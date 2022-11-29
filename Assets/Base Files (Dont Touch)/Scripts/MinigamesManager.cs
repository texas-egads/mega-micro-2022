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

    public Action<MinigameStatus, Action> OnBeginIntermission;
    public Action<MinigameDefinition> OnStartMinigame;
    public Action OnEndMinigame;

    private MinigameStatus status;
    private List<MinigameDefinition> minigames;

    private bool isMinigamePlaying;
    private bool isCurrentMinigameWon;

    // stores a preloaded scene for an upcoming round.
    private AsyncOperation nextMinigameLoadOperation;

    private Coroutine minigameEndCoroutine;

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
        foreach (MinigameDefinition def in allMinigames) {
            if (def.minigameType == MinigameType.Boss)
                boss = def;
            else if (def.minigameType == MinigameType.Miniboss)
                miniboss = def;
            else
                normalMinigames.Add(def);
        }

        if (normalMinigames.Count == 0) {
            Debug.LogError("There are no normal minigames! Please ensure that there is at least one minigame to play.");
        }

        // bag randomize the normal minigames
        normalMinigames.Shuffle();

        // check that we have enough normal minigames to cover the number of rounds
        if (normalMinigames.Count < numberOfRounds) {
            Debug.LogWarning($"There are only {normalMinigames.Count} normal minigames, which isn't enough to fill {numberOfRounds} rounds. This is fine for testing but it shouldn't happen in the actual game.");
        
            int numNormalMinigames = normalMinigames.Count;
            for (int i = numNormalMinigames; i < numberOfRounds; i++) {
                normalMinigames.Add(normalMinigames[i % numNormalMinigames]);
            }
        }

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
        status.totalRounds = minigames.Count;

        status.nextMinigame = GetCurrentMinigameDefinition();
        LoadMinigame(status.nextMinigame);

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

    public void EndCurrentMinigame(float delay = 0) {
        if (!isMinigamePlaying) {
            Debug.LogWarning("EndCurrentMinigame is called when a minigame is not being played. This might happen if you try to call EndCurrentMinigame right after the minigame ran out of time. This call will be ignored.");
            return;
        }

        if (minigameEndCoroutine != null) {
            Debug.LogError("Attempt to call EndCurrentMinigame more than once!");
            return;
        }

        minigameEndCoroutine = StartCoroutine(DoEndMinigame(delay));
    }

    // used by the timer to end a minigame regardless of whether the minigame has been ended by itself
    public void ForceEndCurrentMinigame() {
        if (!isMinigamePlaying) {
            Debug.LogError("Attempt to call ForceEndCurrentMinigame when a minigame is not being played!");
            return;
        }

        if (minigameEndCoroutine != null) {
            StopCoroutine(minigameEndCoroutine);
        }
        minigameEndCoroutine = StartCoroutine(DoEndMinigame(0));
    }

    private IEnumerator DoEndMinigame(float delay) {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        isMinigamePlaying = false;
        OnEndMinigame?.Invoke();

        Managers.__instance.audioManager.FadeMinigameAudio();
        // TODO run a screen wipe and wait for it to finish
        
        SceneManager.UnloadSceneAsync(status.nextMinigame.sceneName);

        UpdateMinigameStatus();
        RunIntermission(status);

        minigameEndCoroutine = null;
    }

    private void UpdateMinigameStatus() {
        status.previousMinigame = status.nextMinigame;
        status.previousMinigameResult = isCurrentMinigameWon ? WinLose.WIN : WinLose.LOSE;

        if (isCurrentMinigameWon) {
            status.healthDelta = 0;
            status.nextRoundNumber = status.nextRoundNumber + 1;

            if (status.previousMinigame.minigameType != MinigameType.Normal)
                status.healthDelta = 1;
        }
        else {
            status.healthDelta = -1;
        }

        status.currentHealth += status.healthDelta;
        if (status.nextRoundNumber >= minigames.Count) {
            status.gameResult = WinLose.WIN;
            status.nextMinigame = null;
        }
        else if (status.currentHealth <= 0) {
            status.gameResult = WinLose.LOSE;
            status.nextMinigame = null;
        }
        else {
            // game still running, proceed with next round
            status.nextMinigame = GetCurrentMinigameDefinition();
            LoadMinigame(status.nextMinigame);
        }
    }


    public void RunIntermission(MinigameStatus status) {
        if (OnBeginIntermission == null) {
            Debug.LogWarning("No one is subscribed to OnBeginIntermission. This is probably a mistake because we expect a listener here to then later call LoadNextMinigame");
        }

        if (status.gameResult == WinLose.NONE) {
            OnBeginIntermission?.Invoke(status, StartNextMinigame);
        }
        else {
            OnBeginIntermission?.Invoke(status, null);
        }
        
    }


    private void LoadMinigame(MinigameDefinition minigameDef) {
        StartCoroutine(DoLoadMinigame(minigameDef));
    }

    private IEnumerator DoLoadMinigame(MinigameDefinition minigameDef) {
        nextMinigameLoadOperation = SceneManager.LoadSceneAsync(minigameDef.sceneName, LoadSceneMode.Additive);
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

        Managers.__instance.audioManager.StartMinigameAudio();
        StartCoroutine(DoStartNextMinigame());
    }

    private IEnumerator DoStartNextMinigame() {
        if (nextMinigameLoadOperation == null) {
            Debug.LogError("Trying to start a minigame, but a minigame scene was never loaded!");
        }

        // wait for the minigame scene to load
        while (nextMinigameLoadOperation.progress < 0.9f)
            yield return null;
        
        nextMinigameLoadOperation.allowSceneActivation = true;
        while (!nextMinigameLoadOperation.isDone)
            yield return null;

        isCurrentMinigameWon = false;
        isMinigamePlaying = true;

        OnStartMinigame?.Invoke(status.nextMinigame);
    }


    public MinigameDefinition GetMinigameDefForScene(Scene scene) {
        return allMinigames.Find(mDef => mDef.sceneName == scene.name);
    }
}
