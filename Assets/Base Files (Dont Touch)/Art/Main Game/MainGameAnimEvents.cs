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
    [SerializeField] private MinigamesManager minigamesManager;

    [SerializeField] private AudioSource bossTheme;

    //public int currentHealthTest = 3;
    //public bool winTest;
    private void Awake()
    {
        UIAnim.Play("A_LifeHold_" + (3));
    }

    public void UpdateUI()
    {
        // Debug.Log(minigamesManager.status.currentHealth);
        // Debug.Log("ui is being updated");

        if (minigamesManager.status.previousMinigameResult == WinLose.WIN)
        {
            //if minigame is won, life UI stays the same
            UIAnim.Play("A_LifeHold_" + (minigamesManager.status.currentHealth));
        }
        else
        {
            //if minigame is lose, add a strike to life UI
            UIAnim.Play("A_Life_" + (minigamesManager.status.currentHealth));
            if (minigamesManager.status.currentHealth <= 0)
            {
                AnimManager.Play("A_lose_gameover");
            }
            
        }
    

    }

    public void PlayBossTheme() {
        bossTheme.Play();
    }

    //played at beginning of lost animation. to keep the last X (mega micro capybara)
    public void MaintainUI() {
        UIAnim.Play("A_LifeHold_" + (minigamesManager.status.currentHealth + 1));
    }

    public void CameraShake()
    {
        CamAnim.Play("A_CameraShake");
    }
}
}