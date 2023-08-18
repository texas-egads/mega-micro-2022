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
    public TextMeshProUGUI promptText;
    public InstructionText instructionText;
    public Image background;

    private Color normalBG;
    [SerializeField] private Color loseBG;
    [SerializeField] private Color winBG;

    private bool oldSpacePressed;
    private Action spacePressedAction;

    public Animator bgAnimator;

    //delay between each minigame
    public float winDelay, loseDelay, introDelay, winToBossDelay, loseToBossDelay;

    public string winAnim, loseAnim, introAnim, winToBossAnim, loseToBossAnim;
    public AudioSource introTheme, winTheme, loseTheme;

    //this will be on my default,
    //turn it off for the actual game implementation.
    public bool debugMode = true;
    [SerializeField] private Text roundText;

    private void Awake() {
        if(background != null) {
            normalBG = background.color;
        } else {
            Debug.Log("Background is Null in MainScene.cs");
        }
        
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


    private void Update() {
        // call the space pressed action whenever space is pressed
        bool spacePressed = Input.GetAxis("Space") > 0;
        if (spacePressed && !oldSpacePressed) {
            spacePressedAction?.Invoke();
            spacePressedAction = null;
        }
        oldSpacePressed = spacePressed;
    }


    private void OnStartMinigame(MinigameDefinition _) {
        container.SetActive(false);
    }

    private void OnEndMinigame() {
        container.SetActive(true);

        // reset the prompt text
        promptText.text = "";
    }

    IEnumerator LoadBossScene(float time) {
        yield return new WaitForSeconds(time);
        //load boss scene with name under the variable "bossScene"
        Managers.__instance.scenesManager.LoadSceneImmediate(Managers.__instance.scenesManager.bossScene);
    }

    private void OnBeginIntermission(MinigameStatus status, Action intermissionFinishedCallback) {
        if(debugMode) {
            // write all of the status to the screen
            statusText.text =
                $"Result of previous minigame: {(status.previousMinigameResult == WinLose.WIN ? "Won" : status.previousMinigameResult == WinLose.LOSE ? "Lost" : "N/A")}\n" +
                $"Rounds completed: {status.nextRoundNumber} out of {status.totalRounds}\n" +
                $"Lives: {status.currentHealth}\n" +
                $"Overall game status: {(status.gameResult == WinLose.WIN ? "Won" : status.gameResult == WinLose.LOSE ? "Lost" : "Playing")}";
        }
        roundText.text = (status.nextRoundNumber + 1).ToString();

        if(status.gameResult == WinLose.WIN) { //move to boss
            if(status.previousMinigameResult == WinLose.WIN) {
                bgAnimator.Play(winToBossAnim);
                winTheme.Play();
                StartCoroutine(LoadBossScene(winToBossDelay));
            } else if(status.previousMinigameResult == WinLose.LOSE) {
                bgAnimator.Play(loseToBossAnim);
                loseTheme.Play();
                StartCoroutine(LoadBossScene(loseToBossDelay));
            }             
            //set active false for parent of the gameobject associated with roundText
            roundText.transform.parent.gameObject.SetActive(false);
            
        }

        else {

            // flash a color if the game was won/lost
            if(debugMode) {
                if (status.previousMinigameResult == WinLose.WIN) {
                    background.color = winBG;
                }
                if (status.previousMinigameResult == WinLose.LOSE) {
                    background.color = loseBG;
                }
            } else { //here, let's decide the animation that is going to play
                //play the animation win/lose/intro
                switch (status.previousMinigameResult) {
                    case WinLose.WIN:
                        bgAnimator.Play(winAnim);
                        winTheme.Play();
                        break;
                    case WinLose.LOSE:
                        bgAnimator.Play(loseAnim);
                        loseTheme.Play();
                        break;
                    default:
                        bgAnimator.Play(introAnim);
                        introTheme.Play();
                        break;
                }
            }

            if(debugMode) {
                if (status.nextMinigame != null) {
                    // prepare for the next minigame
                    DOVirtual.DelayedCall(1f, () => {
                        // return the background color to what it was before
                        background.color = normalBG;

                        // await input
                        promptText.text = "Press SPACE to start next minigame";
                        spacePressedAction = () => OnProceed(status, intermissionFinishedCallback);
                    }, false);
                }
            } else {
                //do like the statement above, except do not use wait on space:
                //just call the action after a delay of 3 seconds.
            //  DOVirtual.DelayedCall(4.66f, () => OnProceed(status, intermissionFinishedCallback), false);
            switch (status.previousMinigameResult) {
                case WinLose.WIN:
                    DOVirtual.DelayedCall(winDelay, () => OnProceed(status, intermissionFinishedCallback), false);
                    break;
                case WinLose.LOSE:
                    DOVirtual.DelayedCall(loseDelay, () => OnProceed(status, intermissionFinishedCallback), false);
                    break;
                default:
                    DOVirtual.DelayedCall(introDelay, () => OnProceed(status, intermissionFinishedCallback), false);
                    break;

            }
            }
        }
    }

    private void OnProceed(MinigameStatus status, Action intermissionFinishedCallback) {
        // start the sequence for the next minigame
        instructionText.ShowImpactText(status.nextMinigame.instruction);
        DOVirtual.DelayedCall(0.5f, () => intermissionFinishedCallback?.Invoke(), false);
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
