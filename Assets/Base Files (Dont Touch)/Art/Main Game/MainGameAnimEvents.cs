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

    public AudioSource loseSound;

    //public int currentHealthTest = 3;
    //public bool winTest;
    private void Awake()
    {
        UIAnim.Play("A_LifeHold_" + (3));
        //set minigamesManager to GameObject find called MinigamesManager
        minigamesManager = GameObject.Find("MinigamesManager").GetComponent<MinigamesManager>();
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
                Debug.Log("HEYY");
                AnimManager.Play("A_lose_gameover");
                StartCoroutine(SetUpGameOver());
                
            }
            
        }
    

    }

    IEnumerator SetUpGameOver() {
        loseSound.Play();
        minigamesManager.status.currentHealth = 3;
        minigamesManager.Initialize();
        Managers.__instance.audioManager.Initialize();
        Managers.__instance.scenesManager.Initialize();
        
        yield return new WaitForSeconds(2f);
        //the load the scene called LoseScreen
        //reset the necessary variables so that the game can be played again
        Managers.__instance.scenesManager.LoadSceneImmediate("LoseScreen");
        
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