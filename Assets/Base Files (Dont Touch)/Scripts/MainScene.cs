using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainScene : MonoBehaviour
{
    public GameObject container;
    public TextMeshProUGUI statusText;
    public InstructionText instructionText;
    public Image background;

    private Color normalBG;
    [SerializeField] private Color loseBG;
    [SerializeField] private Color winBG;

    private void Awake() {
        normalBG = background.color;
    }

    private void Start() {
        Managers.__instance.minigamesManager.OnStartMinigame += OnStartMinigame;
        Managers.__instance.minigamesManager.OnEndMinigame += OnEndMinigame;
        Managers.__instance.minigamesManager.OnBeginIntermission += OnBeginIntermission;

        Managers.__instance.minigamesManager.StartMinigames();
    }

    private void OnDestroy() {
        Managers.__instance.minigamesManager.OnStartMinigame -= OnStartMinigame;
        Managers.__instance.minigamesManager.OnEndMinigame -= OnEndMinigame;
        Managers.__instance.minigamesManager.OnBeginIntermission -= OnBeginIntermission;
    }


    private void OnStartMinigame(MinigameDefinition _) {
        container.SetActive(false);
    }

    private void OnEndMinigame() {
        container.SetActive(true);
    }

    private void OnBeginIntermission(MinigameStatus status, Action intermissionFinishedCallback) {
        statusText.text =
            $"Result of previous minigame: {(status.previousMinigameResult == WinLose.WIN ? "Won" : status.previousMinigameResult == WinLose.LOSE ? "Lost" : "N/A")}\n" +
            $"Rounds completed: {status.nextRoundNumber} out of {status.totalRounds}\n" +
            $"Lives: {status.currentHealth}\n" +
            $"Overall game status: {(status.gameResult == WinLose.WIN ? "Won" : status.gameResult == WinLose.LOSE ? "Lost" : "Playing")}";

        if (status.previousMinigameResult == WinLose.WIN) {
            background.color = winBG;
        }
        if (status.previousMinigameResult == WinLose.LOSE) {
            background.color = loseBG;
        }

        if (status.nextMinigame != null) {
            DOVirtual.DelayedCall(1f, () => background.color = normalBG, false);
            DOVirtual.DelayedCall(2.5f, () => instructionText.ShowImpactText(status.nextMinigame.instruction), false);
        }

        DOVirtual.DelayedCall(3f, () => intermissionFinishedCallback?.Invoke(), false);
    }


    /*
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        MainGameManager.Instance.GrowMainScene += GrowScene;
        MainGameManager.Instance.ShrinkMainScene += ShrinkScene;
    }

    private void GrowScene()
    {
        _animator.Play("main-scene-grow");
    }

    private void ShrinkScene()
    {
        _animator.Play("main-scene-shrink");
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.GrowMainScene -= GrowScene;
        MainGameManager.Instance.ShrinkMainScene -= ShrinkScene;
    }
    */
}
