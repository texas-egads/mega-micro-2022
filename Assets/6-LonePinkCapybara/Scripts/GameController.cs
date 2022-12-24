using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LONEPINKCAPYBARA
{
    public class GameController : MonoBehaviour
    {
        public int selectorPosition; // value of where the cursor is currently pointing
        public int correctIngredient; // value of correct ingredient

        public GameObject selector; // cursor
        public GameObject[] selectorPositions; // empty objects in all three positions where the cursor could be

        public Renderer[] endgameMushrooms; // mesh renderers for the mushroom that falls into the pot
        public Renderer[] UIMushrooms; // renderers for the mushrooms that appear in the speech bubble

        public bool canControl; // can the player manipulate the game via input right now?
        public bool win;

        public Animator cauldronAnimator; // animator for cauldron
        public Animator[] mushroomAnimator; // animators for controlling the rotation of the mushrooms whenever they're selected
        public Animator[] endgameAnimators; // all animators that play during the end sequence

        public ParticleSystem winParticle;
        public ParticleSystem loseParticle;

        public AudioClip bubblingSound; // sound for bubbling cauldron


        // The cursor's position is set to position 1 (center) at the very beginning of the game.
        // The game will then randomly choose one of the ingredients to be the "correct" ingredient.
        // Each ingredient has a value that corresponds with its position: red mushroom = 0, blue mushroom = 1, and yellow mushroom = 2.
        // The UI image of the correct ingredient will then appear after it has been randomly chosen.
        // canControl determines whether the player can manipulate the game via input. This is set to "true" at the start of the game.
        void Start()
        {
            selectorPosition = 1;
            selector.transform.position = selectorPositions[selectorPosition].transform.position;
            mushroomAnimator[selectorPosition].SetTrigger("Select");

            correctIngredient = UnityEngine.Random.Range(0, 3);
            Debug.Log("" + correctIngredient);

            UIMushrooms[correctIngredient].enabled = true;

            AudioSource bubbling = Managers.AudioManager.CreateAudioSource();
            bubbling.PlayOneShot(bubblingSound);

            canControl = true;
        }

        // The only things in Update() are checking for key inputs.
        // D and A move the cursor right and left respectively.
        // When spacebar is pressed, the game will check if the player has won or lost the game.
        void Update()
        {
            if (canControl == true)
            {
                if (Input.GetKeyDown(KeyCode.D)) { MoveSelectorRight(); }
                if (Input.GetKeyDown(KeyCode.A)) { MoveSelectorLeft(); }

                if (Input.GetKeyDown(KeyCode.Space)) { CheckIfGameWin(); }
            }
            
        }

        // MoveSelectorRight() and MoveSelectorLeft() just move the cursor to the proper position, either to the right or to the left.
        // If the player would move the cursor beyond the possible range of positions, they will be looped back around to the other side. 
        void MoveSelectorRight()
        {
            mushroomAnimator[selectorPosition].SetTrigger("Deselect");
            selectorPosition++;
            if (selectorPosition > 2) { selectorPosition = 0; }
            selector.transform.position = selectorPositions[selectorPosition].transform.position;
            mushroomAnimator[selectorPosition].SetTrigger("Select");
        }
        void MoveSelectorLeft()
        {
            mushroomAnimator[selectorPosition].SetTrigger("Deselect");
            selectorPosition--;
            if (selectorPosition < 0) { selectorPosition = 2; }
            selector.transform.position = selectorPositions[selectorPosition].transform.position;
            mushroomAnimator[selectorPosition].SetTrigger("Select");
        }


        // CheckIfGameWin() is triggered when the player presses the spacebar.
        // Whether or not the player chose correctly, control is disabled.
        // Also, animations are played for the end-of-game sequence.
        // If the cursor position and the "correct ingredient" value are a match, the player wins.
        // otherwise, the player loses.
        void CheckIfGameWin()
        {
            canControl = false;

            endgameMushrooms[selectorPosition].enabled = true;

            foreach (Animator endgameAnimator in endgameAnimators) { endgameAnimator.SetTrigger("End"); }
            cauldronAnimator.SetTrigger("End");

            if (selectorPosition == correctIngredient) { WinGame(); }
            else { LoseGame(); }
        }

        void WinGame()
        {
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            win = true;
            cauldronAnimator.SetBool("Win", true);
            endgameAnimators[2].SetBool("Win", true);
            winParticle.Play(); 
        }

        void LoseGame()
        {
            win = false;
            cauldronAnimator.SetBool("Lose", true);
            endgameAnimators[2].SetBool("Lose", true);
            loseParticle.Play();
        }
    }
}

