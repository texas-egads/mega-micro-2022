using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Babybaras
{


public class Capybarascript : MonoBehaviour
{
    AudioSource swingwhoosh;
    bool playedThrowSound = false;
    AudioSource music;
    

    // Start is called before the first frame update
    void Start()
    {
        swingwhoosh = Managers.AudioManager.CreateAudioSource();
        playedThrowSound = false;

        music = Managers.AudioManager.CreateAudioSource();
        music.clip = Resources.Load("Babybaras music") as AudioClip;
        music.Play();
        Debug.Log(music.isPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        if (Bugvars.lassoing == false)
        {
            swingwhoosh.clip = Resources.Load("245933__sophiehall3535__light-woosh") as AudioClip;
            if(swingwhoosh.isPlaying == false)
            {
                swingwhoosh.time = 1.5f;
                swingwhoosh.Play();
            }

            playedThrowSound = false;
        
        }
        else {
            swingwhoosh.clip = Resources.Load("60013__qubodup__whoosh") as AudioClip;
            if (playedThrowSound == false)
            {

                swingwhoosh.Play();
                playedThrowSound = true;
            }

        }
        
    }
}

}
