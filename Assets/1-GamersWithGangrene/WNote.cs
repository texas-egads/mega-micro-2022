using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamersWithGangrene
{
    public class WNote : MonoBehaviour
    {
        GameObject NoteHandler;
        bool noteAlreadyPressed;

        Animator anim;
        float animStart;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            animStart = -1;
            NoteHandler = GameObject.Find("NoteHandler");
             
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Vertical") > 0 && (transform.position.y > 210 && transform.position.y < 240))
            {
                anim.Play("hit");
                animStart = Time.time;

                if (!noteAlreadyPressed)
                {
                    NoteHandler.GetComponent<NoteFall>().IncrementNotesPressed();
                    noteAlreadyPressed = true;
                }
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
