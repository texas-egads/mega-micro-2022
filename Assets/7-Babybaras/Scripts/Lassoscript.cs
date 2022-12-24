using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Babybaras
{
    public class Bugvars
    {
        public static bool gotWaterbug = false;
        public static bool gotFirebug = false;
        public static bool gotLightningbug = false;
        public static int throwing = 1; // throwing is 1 is throwing, -1 if pulling
        public static bool lassoing = false;
        public static AudioSource bugSounds = Managers.AudioManager.CreateAudioSource();
        public static AudioSource catchSounds = Managers.AudioManager.CreateAudioSource();

    }
    

    public class Lassoscript : MonoBehaviour
    {   
        //Bugvars.lassoing = false;
        static float angleCounter = 0;
        float twoPi = 2 * Mathf.PI;
        Vector3 throwVector;
        Vector3 basePos;
        
        // Start is called before the first frame update
        void Start()
        {
            basePos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            //basePos = basePosEx;
        }

        // Update is called once per frame
        void Update()
        {
            angleCounter += twoPi * Time.deltaTime * 2;
            angleCounter %= twoPi;
            //transform.Rotate(new Vector3(0, ))
            if (Input.GetButtonDown("Space") && (Bugvars.lassoing == false)){
                throwVector = new Vector3(3 * Mathf.Cos(angleCounter), 3 * Mathf.Sin(angleCounter), 0);
                Bugvars.lassoing = true;
            }

            if (Bugvars.lassoing == true){
                transform.Translate(Bugvars.throwing * throwVector, Space.Self);
                //Debug.Log(throwing * throwVector);

            }
            else {
                float viewAngle = (angleCounter - (Mathf.PI / 2));
                transform.position = basePos + new Vector3(15 * Mathf.Cos(viewAngle), 15 * Mathf.Sin(viewAngle), 0);
            }


            

        }

        void OnTriggerExit2D(Collider2D collision){
                if ((collision.gameObject.tag == "Ground") && (Bugvars.lassoing == true) && (Bugvars.throwing == 1)){
                    Bugvars.throwing = -1;
                    
                }
            
            }
            

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.tag == "Player") {
                Bugvars.lassoing = false;
                Bugvars.throwing = 1;
                transform.position = collision.gameObject.transform.position;
            }
        }

        /*  void OnTriggerStay2D(Collider2D collision) {
            if (collision.gameObject.tag == "player") {
                lassoing = false;
                throwing = 1;
            }
            Debug.Log("On trigger stay");
        } */

        public static float getLassoAngle() 
        {
            return angleCounter;
        }


    }

    
}