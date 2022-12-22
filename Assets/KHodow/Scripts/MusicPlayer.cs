using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHodow
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioClip loopSound;

        private void Start()
        {            
            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.volume = 0.5f;
            loop.clip = loopSound;
            loop.Play();
        }

        private void Update()
        {

        }
    }
}

