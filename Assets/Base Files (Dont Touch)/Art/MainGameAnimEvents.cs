using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameArt
{
public class MainGameAnimEvents : MonoBehaviour
{
    //[SerializeField] private ParticleSystem potion;
    private MinigameStatus status;
    /*void ResetParticleSystem()
    {
        potion.Clear();
    }*/
    [SerializeField] private Animator UIAnim;

    public int currentHealthTest = 3;
    public bool winTest;
    private void Awake()
    {
        UIAnim.Play("A_LifeHold_" + (currentHealthTest));
    }
    public void UpdateUI()
    {
        Debug.Log("ui is being updated");

        if (winTest == true) //status.previousMinigameResult == WinLose.WIN)
        {
            UIAnim.Play("A_LifeHold_" + (currentHealthTest));
        }
        else
        {
            UIAnim.Play("A_Life_" + (currentHealthTest));
        }

    }
}
}