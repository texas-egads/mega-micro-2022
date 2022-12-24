using UnityEngine;
using UnityEngine.UI;

namespace Wombies
{
    public class Counter : MonoBehaviour
    {
        private int hits = 0;
        public Text hitsText;
        Animator anim;
        public AudioClip backgroundMusic;
        public AudioClip winMusic;

        private void Start()
        {
            //start zombie off
            //this.gameObject.GetComponent<Renderer>().enabled = false; //anim still plays at start, not solution
            anim = GetComponent<Animator>();
            anim.SetBool("isAlive", false); //anim controller parameter
            AudioSource musicloop = Managers.AudioManager.CreateAudioSource();
            musicloop.loop = true;
            musicloop.clip = backgroundMusic;
            musicloop.Play();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetButtonDown("Space"))
            {
                hits += 1;
            }

            //end game once hit count is met
            if (hits == 15)
            {
                //Debug.Log("you won omg");
                //zombiebara anim and music
                //this.gameObject.GetComponent<Renderer>().enabled = true;
                anim.SetBool("isAlive", true);

                this.enabled = false;

                AudioSource winSound = Managers.AudioManager.CreateAudioSource();
                winSound.PlayOneShot(winMusic);

                // from example script to end game
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(1.5f);
            }

            //keep track of hits; del for final ver
            hitsText.text = hits.ToString("spacebar hits: 0");
        }
    }
}

