using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LONEPINKCAPYBARA
{
    public class mushroomSoundEvents : MonoBehaviour
    {
        public AudioClip fallSound;
        public AudioClip splashSound;
        public AudioClip winSound;
        public AudioClip loseSound;

        public GameObject gameController;

        public void playFallSound()
        {
            AudioSource fall = Managers.AudioManager.CreateAudioSource();
            fall.PlayOneShot(fallSound);
        }

        public void playSplashSound()
        {
            AudioSource splash = Managers.AudioManager.CreateAudioSource();
            splash.PlayOneShot(splashSound);
        }

        public void playEndSound()
        {
            if (gameController.GetComponent<GameController>().win == true)
            {
                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.PlayOneShot(winSound);
            }
            else
            {
                AudioSource lose = Managers.AudioManager.CreateAudioSource();
                lose.PlayOneShot(loseSound);
            }
            
        }

    }
}

