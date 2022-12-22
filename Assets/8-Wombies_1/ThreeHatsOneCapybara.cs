using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wombies {
    public class ThreeHatsOneCapybara : MonoBehaviour {


        // Text
        public TextMeshProUGUI uiText;
        public string startText;
        public string winText;

        // Sound
        public AudioClip winSound;

        // positions must be between 0 and 4 inclusive
        private int currentPos = 3;
        private int winningPos;

        private void Start()
        {
            // randomly assigning 
            winningPos = UnityEngine.Random.Range(0, 5);

            // start text
            uiText.text = startText;

        }

        private void Update()
        {
            // keep track of current position
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentPos < 4)
            {
                // move along x axis by how ever much
                // in our case 4 units (right)
                currentPos++;
            }

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentPos > 0)
            {
                currentPos--;
            }

            // space bar interaction
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // reveal what's underneath the hat
                if (this.gameObject.transform.GetChild(currentPos).GetComponent<Renderer>().enabled)
                {
                    this.gameObject.transform.GetChild(currentPos).GetComponent<Renderer>().enabled = false;
                }

                // if we pick the correct one (win) 
                if (currentPos == winningPos)
                {
                    // render capybara for winning hat
                    this.gameObject.transform.GetChild(currentPos).GetChild(0).GetComponent<Renderer>().enabled = true;

                    // play sound
                    AudioSource win = Managers.AudioManager.CreateAudioSource();
                    win.PlayOneShot(winSound);
                    
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    Managers.MinigamesManager.EndCurrentMinigame(1.2f);
                }
            }
        }
    }
}
