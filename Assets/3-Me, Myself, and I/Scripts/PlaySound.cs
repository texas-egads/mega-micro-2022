using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class PlaySound : MonoBehaviour
    {

        public AudioClip sound;
        public float volume;

        void Start()
        {
            AudioSource m = Managers.AudioManager.CreateAudioSource();
            m.clip = sound;
            m.volume = volume;
            m.Play();
        }
    }


}