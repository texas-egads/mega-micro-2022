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
        public bool freeze;

        private void Start()
        {
            noliteAnim = GetComponent<Animator>();
            noroiBox = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (Input.GetButton("Space"))
            {
                noroiBox.enabled = false;
                noliteAnim.ResetTrigger("notTransform");
                noliteAnim.SetTrigger("transform");

                //StartCoroutine(punishmentForce());

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;

                //Transfrom when holding down SPACEBAR
                //Set Off Animation
                //Disable Box Collider
            }
            else
            {
                noroiBox.enabled = true;
                noliteAnim.ResetTrigger("transform");
                noliteAnim.SetTrigger("notTransform");

                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //When let go of SPACEBAR, detransform
                //Set Off Animation
                //Enable Box Collider
            }
            rb.AddForce(transform.right * force);

        }

        IEnumerator punishmentForce()
        {
           rb.constraints = RigidbodyConstraints2D.None;
           rb.constraints = RigidbodyConstraints2D.FreezeRotation;
           rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            yield return new WaitForSeconds(4);
        }

  }
}

//|| Input.GetButtonDown("Space")