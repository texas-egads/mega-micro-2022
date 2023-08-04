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

    private Animator fadeAnimator;

    private void StartTimer(MinigameDefinition minigameDef) {
        if (minigameDef.gameTime == MinigameLength.Uncapped)
            return;
        
        timerCoroutine = StartCoroutine(DoTimer((int)minigameDef.gameTime / 1000f));
        timerUI.Activate();
    }

    private bool transitioning = false;

    private void EndTimer() {
        if (timerCoroutine != null) {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        transitioning = false;
        timerUI.Deactivate();
        
    }

    private void FadeOut() {
        fadeAnimator = GameObject.Find("Transitioner").GetComponent<Animator>();
        transitioning = true;
        //only play if it is not already playing
        if (fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("mask-shrink") == false) {
            fadeAnimator.Play("mask-shrink");
        }
    }

    private IEnumerator DoTimer(float time) {
        while (time > 0) {
            timerUI.ShowTime(time);
            yield return null;
            time -= Time.deltaTime;
            if(time <= 0.333f && !transitioning) {
                FadeOut();
            }
        }
        timerUI.ShowTime(0);

        Managers.__instance.minigamesManager.ForceEndCurrentMinigame();
    }

    
}
