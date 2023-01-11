using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamersWithGangrene
{
    public class NoteFall : MonoBehaviour
    {
        int notesPressed;
        // Start is called before the first frame update
        void Start()
        {
            notesPressed = 0;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(0, -60 * Time.deltaTime, 0);
        }

        public void IncrementNotesPressed()
        {

            notesPressed++;
        }

        public int GetNotesPressed()
        {
            return notesPressed;
        }
    }

}