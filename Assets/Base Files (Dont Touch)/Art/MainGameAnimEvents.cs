using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameArt
{
public class MainGameAnimEvents : MonoBehaviour
{
    private MinigameStatus status;
    [SerializeField] private Animator UIAnim; //animator on Background > Animation Manager > Chalkboard > Lives
    [SerializeField] private Animator AnimManager; //animator on Background > Animation Manager
    [SerializeField] private Animator CamAnim; //animator on Background

    //public int currentHealthTest = 3;
    //public bool winTest;
    private void Awake()
    {
        UIAnim.Play("A_LifeHold_" + (status.currentHealth));
    }
    public void UpdateUI()
    {
        Debug.Log("ui is being updated");

        if (status.previousMinigameResult == WinLose.WIN)
        {
            //if minigame is won, life UI stays the same
            UIAnim.Play("A_LifeHold_" + (status.currentHealth));
        }
        else
        {
            //if minigame is lose, add a strike to life UI
            UIAnim.Play("A_Life_" + (status.currentHealth));
            if (status.currentHealth <= 0)
            {
                AnimManager.Play("A_lose_gameover");
            }
        }

    }
    public void CameraShake()
    {
        CamAnim.Play("A_CameraShake");
    }
}
}