using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class YouDied : MonoBehaviour
    {
        public SpriteRenderer sr;

        private void Start()
        {
            if (sr.color.a != 0)
            {
                Destroy(gameObject);
            }
        }
        private void FixedUpdate()
        {
            if (sr.color.a < 1)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + 0.01f);
            }
        }
    }
}
