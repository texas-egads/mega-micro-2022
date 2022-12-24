using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class Death : MonoBehaviour
    {
        public PlayerMovement pm;
        public SpriteRenderer sr;
        public GameObject smoke;
        public GameObject death;
        public BoxCollider2D bc;
        public BossAI bAI;
        public AudioClip dead;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Object 1"))
            {
                AudioSource m = Managers.AudioManager.CreateAudioSource();
                m.clip = dead;
                m.volume = 0.5f;
                m.Play();
                pm.enabled = false;
                sr.enabled = false;
                bAI.notDead = false;
                Instantiate(smoke, transform.position, Quaternion.Euler(-90, 0, 0));
                bc.transform.position = new Vector3(0, -100, 0);
                Instantiate(death, new Vector3(0, 0.15f, 0), Quaternion.identity);
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(3f);
            }
        }
    }
}

