using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TNDStudios
{
    public class Wizard : MonoBehaviour
    {
        public AudioClip deathScream; 
        public GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                transform.Translate(new Vector3(0, 0, 0.5f));
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                try
                {
                    AudioSource screamSource = Managers.AudioManager.CreateAudioSource();
                    if (screamSource != null)
                    {
                        screamSource.clip = deathScream;
                        screamSource.Play();
                    }
                }
                catch { }
                gameManager.wizardsKilled++; 

            }
        }
    }
}
