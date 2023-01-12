using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
    public class BG_Repeat : MonoBehaviour
    {
        private BoxCollider2D bxC;
        private Rigidbody2D rb;

        private float width;
        private float speed = -1f; 
        void Start()
        {
            bxC = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();

            width = bxC.size.x;
            rb.velocity = new Vector2(speed, 0);
        }

        void Update()
        {
            if(transform.position.x <-width)
            {
                Reposition();
            }
        }

        private void Reposition()
        {
            Vector2 vector = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + vector;
        }
    }
}
