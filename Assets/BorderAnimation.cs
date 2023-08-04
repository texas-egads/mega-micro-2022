using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderAnimation : MonoBehaviour
{

    public float animationLength = 2.0f;
    private Animator animator;
    //this script is meant to automatically adjust the animation speed based on game length
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //find object with name MinigamesManager:
        MinigamesManager minigamesManager = GameObject.Find("MinigamesManager").GetComponent<MinigamesManager>();
        int gameLength = (int) minigamesManager.status.nextMinigame.gameTime;
        //consider the current speed of the animation state called animationName, and adjust the speed of the animator to fit the game length

        //get length of animation state called animationName
        
        animator.speed = animationLength / (gameLength / 1000.0f);
    }

    
}
