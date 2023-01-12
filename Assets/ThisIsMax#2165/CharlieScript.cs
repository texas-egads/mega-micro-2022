using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThisIsMax
{
    public class CharlieScript : MonoBehaviour
    {
        public Animator animator;
        // Update is called once per frame
        void Update()
        {
            // Check when done with animation
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Destroy the game object
                animator.enabled = false;
                // Move him up
                gameObject.transform.position += new Vector3(0, 0.01f, 0);
            }
        }
    }
}