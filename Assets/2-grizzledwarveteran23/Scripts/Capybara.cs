using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class Capybara : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        private Rigidbody2D rb;

        private bool dying = false;

        private AudioSource musicLoop;
        public AudioClip music;
        public AudioClip deathSound;
        // Start is called before the first frame update
        void Start()
        {
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            rb = GetComponent<Rigidbody2D>();
            musicLoop = Managers.AudioManager.CreateAudioSource();
            musicLoop.loop = true;
            musicLoop.clip = music;
            musicLoop.Play();
        }


        void FixedUpdate()
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            move = move.normalized * speed;
            rb.velocity = move;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.name.Contains("Enemy") && !dying)
            {
                dying = true;
                rb.velocity = Vector2.zero;
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(1.5f);
                this.enabled = false;
                AudioSource death = Managers.AudioManager.CreateAudioSource();
                musicLoop.Stop();
                death.PlayOneShot(deathSound);
                StartCoroutine(flipOut());
            }
        }

        IEnumerator flipOut() {
            //this will flip the player across all axis chaotically and fall because it has died
            for(int i = 0; i < 100; i++) {
                transform.Rotate(Random.Range(-10, 20), Random.Range(-20, 5), Random.Range(10, 10));
                yield return new WaitForSeconds(0.01f);
            }
            

        }
    }
}