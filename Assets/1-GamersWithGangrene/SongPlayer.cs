using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamersWithGangrene
{
    public class SongPlayer : MonoBehaviour
    {
        public AudioClip song;
        // Start is called before the first frame update
        void Start()
        {
            AudioSource source = Managers.AudioManager.CreateAudioSource();
            source.clip = song;
            source.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
