using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
  public class Player_Transform : MonoBehaviour
    {
      Animator NoliteAnim;
        private void Start()
        {
            NoliteAnim = GetComponent<Animator>();
        }
        void Update()
       {

            //Transfrom when holding down SPACEBAR
            //Set Off Animation
            //Disable Box Collider

            //When let go of SPACEBAR, detransform
            //Set Off Animation
            //Enable Box Collider

        }



    }
}

