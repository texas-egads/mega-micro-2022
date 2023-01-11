using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class Moon : MonoBehaviour
    {
        public Transform player;
        public GameObject particle;
        public float speed;
        public float downSpeed;
        public AudioClip rumble;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            AudioSource m = Managers.AudioManager.CreateAudioSource();
            m.clip = rumble;
            m.Play();
        }

        private void FixedUpdate()
        {
            transform.position += new Vector3(0, -downSpeed);
            if(transform.position.x > player.position.x)
            {
                transform.position += new Vector3(-speed, 0);
            }
            else
            {
                transform.position += new Vector3(speed, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision2D)
        {
            if (collision2D.gameObject.CompareTag("Ground"))
            {
                Instantiate(particle, transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
                Destroy(gameObject);
            }
        }
    }
}
