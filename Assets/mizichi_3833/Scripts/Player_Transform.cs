using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
  public class Player_Transform : MonoBehaviour
  {
        private Animator noliteAnim;
        private BoxCollider2D noroiBox;
        private Rigidbody2D rb;
        public float force = -10;

        public AudioClip transAudio;
        public AudioClip loopAudio;
        private void Start()
        {
            noliteAnim = GetComponent<Animator>();
            noroiBox = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopAudio;
            loop.Play();
        }

        void Update()
        {
            AudioSource transformAudio = Managers.AudioManager.CreateAudioSource();
            transformAudio.loop = false;
            transformAudio.clip = transAudio;

            if (Input.GetButton("Space"))
            {
                noroiBox.enabled = false;
                noliteAnim.ResetTrigger("notTransform");
                noliteAnim.SetTrigger("transform");
                transformAudio.Play();


                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                noroiBox.enabled = true;
                noliteAnim.ResetTrigger("transform");
                noliteAnim.SetTrigger("notTransform");

                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            rb.AddForce(transform.right * force);

        }

  }
}
