using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainScene : MonoBehaviour
{

    public TextMeshProUGUI statusText;

    private void Start() {
        Managers.instance.minigamesManager.StartMinigames();
    }

    private void OnEnable() {
        Managers.instance.minigamesManager.OnStartMinigame += OnStartMinigame;
        Managers.instance.minigamesManager.OnEndMinigame += OnEndMinigame;
        Managers.instance.minigamesManager.OnBeginIntermission += OnBeginIntermission;
    }

    private void OnDisable() {
        Managers.instance.minigamesManager.OnStartMinigame -= OnStartMinigame;
        Managers.instance.minigamesManager.OnEndMinigame -= OnEndMinigame;
        Managers.instance.minigamesManager.OnBeginIntermission -= OnBeginIntermission;
    }


    private void OnStartMinigame() {

    }

    private void OnEndMinigame() {

    }

    private void OnBeginIntermission(MinigameStatus status, Action intermissionFinishedCallback) {
        statusText.text =
            $"Previous Minigame: {(status.previousMinigameResult == WinLose.WIN ? "Won" : status.previousMinigameResult == WinLose.LOSE ? "Lost" : "N/A")}\n" +
            $"Round: {status.nextRoundNumber}\n" +
            $"Lives: {status.currentHealth}\n" +
            $"Game Status: {(status.gameResult == WinLose.WIN ? "Won" : status.gameResult == WinLose.LOSE ? "Lost" : "Playing")}";

        DOVirtual.DelayedCall(3f, () => intermissionFinishedCallback?.Invoke());
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
