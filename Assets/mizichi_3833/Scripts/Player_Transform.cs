using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
  public class Player_Transform : MonoBehaviour
    {
        private Animator NoliteAnim;
        private BoxCollider2D noroiBox;

        private void Start()
        {
            NoliteAnim = GetComponent<Animator>();
            noroiBox = GetComponent<BoxCollider2D>();
        }
        void Update()
       {
            if (Input.GetButton("Space"))
            {
                noroiBox.enabled = false;
                //Transfrom when holding down SPACEBAR
                //Set Off Animation
                //Disable Box Collider
            }
            else
            {
                noroiBox.enabled = true;

                //When let go of SPACEBAR, detransform
                //Set Off Animation
                //Enable Box Collider
            }


        }



    }
}

