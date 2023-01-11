using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamersWithGangrene
{
    public class SpaceNote : MonoBehaviour
    {
        Animator anim;
        float animStart;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            animStart = -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Space") && (transform.position.y > 210 && transform.position.y < 240))
            {
                anim.Play("Spacehit");
                animStart = Time.time;
            }

            if (animStart != -1)
            {
                if (Time.time >= animStart + .1)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
