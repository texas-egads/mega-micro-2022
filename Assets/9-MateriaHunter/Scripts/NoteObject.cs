using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MateriaHunter {

    public class NoteObject : MonoBehaviour
    {
        public bool canBePressed;
       // private bool obtained = false;
        public KeyCode keyToPress;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(keyToPress)) {
                if(canBePressed){
                    
               
                    gameObject.SetActive(false);
                    GameManager.instance.NoteHit();
                    
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Object 1") {
                canBePressed = true;
                
            }

        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.tag == "Object 1" && gameObject.activeSelf) {
                canBePressed = false;
                
                    GameManager.instance.NoteMissed();
                
            }

        }
    }
}