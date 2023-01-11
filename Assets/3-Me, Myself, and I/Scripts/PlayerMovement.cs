using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class PlayerMovement : MonoBehaviour
    {
        public bool dodgeTimer;
        public bool grounded;
        public Rigidbody2D rb;
        public float power;
        public float timeWait;
        public Animator anim;
        float initScale;
        public AudioClip bossMusic;

        private void Start()
        {
            AudioSource m = Managers.AudioManager.CreateAudioSource();
            m.clip = bossMusic;
            m.volume = 0.3f;
            m.Play();
            dodgeTimer = true;
            initScale = transform.localScale.x;

        }

        private void Update()
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            if (Input.GetAxis("Space") > 0)
            {
                ver = 1;
            }

            if (dodgeTimer && hor > 0 && grounded)
            {
                anim.SetBool("Dodge", true);
                Dodge(2);
            }
            if (dodgeTimer && hor < 0 && grounded)
            {
                anim.SetBool("Dodge", true);
                Dodge(1);
            }
            if (dodgeTimer && ver > 0 && grounded)
            {
                anim.SetBool("Jump", true);
                Dodge(3);
            }


        }

        private void Dodge(int dir)
        {
            if (dir == 1) //Left
            {
                dodgeTimer = false;
                rb.AddForce(new Vector2(-power, 0));
                StartCoroutine(DodgeWaitHor(timeWait));
            }
            else if (dir == 2) //Right
            {
                dodgeTimer = false;
                transform.localScale = new Vector3(initScale * -1, transform.localScale.y, transform.localScale.z);
                rb.AddForce(new Vector2(power, 0));
                StartCoroutine(DodgeWaitHor(timeWait));
            }
            else if (dir == 3) //Up
            {
                grounded = false;
                rb.AddForce(new Vector2(0, power));
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.CompareTag("Ground"))
            {
                grounded = true;
                anim.SetBool("Jump", false);
            }
        }

        private void ResetDodgeHor()
        {
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("Dodge", false);
            transform.localScale = new Vector3(initScale, transform.localScale.y, transform.localScale.z);
            StartCoroutine(DodgeCool(0.1f));
        }

        IEnumerator DodgeWaitHor(float t)
        {
            yield return new WaitForSeconds(t);
            ResetDodgeHor();
        }

        IEnumerator DodgeCool(float t)
        {
            yield return new WaitForSeconds(t);
            dodgeTimer = true;
        }
    }
}