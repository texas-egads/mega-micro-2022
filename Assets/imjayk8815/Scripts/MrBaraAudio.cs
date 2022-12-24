using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace imjayk8815
{
    public class MrBaraAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip music;

        // Start is called before the first frame update
        void Start()
        {
            Managers.AudioManager.CreateAudioSource().PlayOneShot(music, .25f);
        }
    }
}
