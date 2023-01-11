using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GamersWithGangrene
{
    public class Rotator : MonoBehaviour
    {
        float startTime;

        GameObject NoteHandler;

        public AudioClip loopSound;
        public AudioClip winSound;

        Vector3 dest;
        // Start is called before the first frame update
        void Start()
        {
            startTime = -1;

            NoteHandler = GameObject.Find("NoteHandler");

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopSound;
            loop.Play();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetButtonDown("Space"))
            {
                if (startTime == -1)
                {
                    startTime = Time.time;
                }
            }

            if (startTime != -1)
            {
                if (NoteHandler.GetComponent<NoteFall>().GetNotesPressed() > 10)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                    PlayWin();
                } else
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                    Managers.MinigamesManager.EndCurrentMinigame(2f);
                    PlayLoss();
                }

                
            }

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            if (vertical == 0 && horizontal == 0) // WAS or D pressed
            {
                return;
            }

            float rotation;
            if (vertical > 0)
            {
                rotation = 90;
            }
            else if (vertical < 0)
            {
                rotation = 270;
            }
            else if (horizontal > 0)
            {
                rotation = 180;
            }
            else // (horizontal < 0)
            {
                rotation = 0;
            }

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        void PlayWin()
        {
            if (Time.time - startTime < .1)
            {
                transform.Translate(500 * Time.deltaTime, 0, 0);
            }
            else if (Time.time - startTime < 1 && Time.time - startTime > .3)
            {
                transform.Translate(-5000 * Time.deltaTime, 0, 0);
            }
        }

        void PlayLoss()
        {
            if (Time.time - startTime < 2)
            {
                transform.Translate(0, -100 * Time.deltaTime, 0);
                transform.Rotate(0, 300 * Time.deltaTime, 0);
            }
        }
    }
}
