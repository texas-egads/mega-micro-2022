using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Babybaras
{
    public class Waterbugscript : MonoBehaviour
{
    int i;
    Vector3 wiggle = new Vector3(0, 0, 5);
    int wiggleflip = 1;
    AudioSource bugSounds;
    AudioSource catchSounds;



    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        bugSounds = Managers.AudioManager.CreateAudioSource();
        catchSounds = Managers.AudioManager.CreateAudioSource();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Bugvars.gotWaterbug == false) {    
            if (i == 0){
                transform.Translate(1, 5, 0, Space.Self);
            }
            i++;
            i %= 70;

            if (i % 3 == 0){
            transform.Rotate(2 * wiggleflip * wiggle);
            wiggleflip *= -1;
        }
        }
        
        
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "GameController" && Bugvars.throwing == 1 && Bugvars.gotWaterbug == false){
            Bugvars.gotWaterbug = true;
            Bugvars.throwing = -1;
            transform.position = GameObject.FindGameObjectsWithTag("Finish")[0].transform.position + new Vector3(-10, 0, 0);
            bugSounds.clip = Resources.Load("371274__mafon2__water-click") as AudioClip;
            bugSounds.Play();
            catchSounds.clip = Resources.Load("650943__ajanhallinta__pickupsfx") as AudioClip;
            catchSounds.Play();

            if ((Bugvars.gotWaterbug == true) && (Bugvars.gotLightningbug == true) && (Bugvars.gotFirebug == true)) {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(1);
                catchSounds.clip = Resources.Load("325805__wagna__collect") as AudioClip;
                catchSounds.Play();
            }
        }

    }
}
}

