using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace imjayk8815
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private GameObject explosionParticles;
        [SerializeField] private AudioClip explosionSound;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * projectileSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.tag == "Object 1")
            {
                Instantiate(explosionParticles, transform.position, Quaternion.identity);
                Managers.AudioManager.CreateAudioSource().PlayOneShot(explosionSound, .25f);
                Destroy(gameObject);
            }
        }
    }
}