using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private void OnEnable() {
        Managers.__instance.minigamesManager.OnStartMinigame += StartTimer;
        Managers.__instance.minigamesManager.OnEndMinigame += EndTimer;
    }

    private void OnDisable() {
        Managers.__instance.minigamesManager.OnStartMinigame -= StartTimer;
        Managers.__instance.minigamesManager.OnEndMinigame -= EndTimer;
    }

    [SerializeField] private TimerUI timerUI = default;
    private Coroutine timerCoroutine;

    private void StartTimer(MinigameDefinition minigameDef) {
        if (minigameDef.gameTime == MinigameLength.Uncapped)
            return;
        
        timerCoroutine = StartCoroutine(DoTimer((int)minigameDef.gameTime / 1000f));
        timerUI.Activate();
    }

    private void EndTimer() {
        if (timerCoroutine != null) {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        
        timerUI.Deactivate();
    }

    private IEnumerator DoTimer(float time) {
        while (time > 0) {
            timerUI.ShowTime(time);
            yield return null;
            time -= Time.deltaTime;
        }
        timerUI.ShowTime(0);

        Managers.__instance.minigamesManager.ForceEndCurrentMinigame();
    }

    
}
