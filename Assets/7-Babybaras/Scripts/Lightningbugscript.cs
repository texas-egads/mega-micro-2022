using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Babybaras
{
    public class Lightningbugscript : MonoBehaviour
{
    int i;
    Vector3 wiggle = new Vector3(0, 0, 5);
    int wiggleflip = 1;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;

        transform.Rotate(wiggle);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Bugvars.gotLightningbug == false) {    
            if (i == 0){
                transform.Translate(1, -1, 0, Space.Self);
            }
            i++;
            i %= 20;
            if (i % 3 == 0){
                transform.Rotate(2 * wiggleflip * wiggle);
                wiggleflip *= -1;
            }
            
        }

        
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "GameController" && Bugvars.throwing == 1 && Bugvars.gotLightningbug == false){
            Bugvars.gotLightningbug = true;
            Bugvars.throwing = -1;
            transform.position = GameObject.FindGameObjectsWithTag("Finish")[0].transform.position + new Vector3(20, 0, 0);
            transform.Rotate(0, 0, 70);
            Bugvars.bugSounds.clip = Resources.Load("512471__michael-grinnell__electric-zap") as AudioClip;
            Bugvars.bugSounds.Play();
            Bugvars.catchSounds.clip = Resources.Load("650943__ajanhallinta__pickupsfx") as AudioClip;
            Bugvars.catchSounds.Play();

            if (Bugvars.gotWaterbug && Bugvars.gotLightningbug && Bugvars.gotFirebug) {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(1);
                Bugvars.catchSounds.clip = Resources.Load("325805__wagna__collect") as AudioClip;
                Bugvars.catchSounds.Play();
            }
        }
    }
}
}

