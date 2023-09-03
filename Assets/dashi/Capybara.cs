using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dashi
{
    public class Capybara : MonoBehaviour
    {
        [SerializeField] private dashi.GameManager _manager;
        private bool _gameDone = false;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!_gameDone) {
                //get all objects called Bee Object (Clone). that will be the name. not the tag
                GameObject[] bees = GameObject.FindGameObjectsWithTag("Bee");
                //print length of array
                //check if the capybara is colliding with any of the bees (inside the bee's collider bounds)
                //iterate through bees
                foreach (GameObject bee in bees)
                {
                    //check if player position is within the bounds of the bee. just compare the positions of the player and the bee
                    //get the position of the bee
                    Vector3 beePosition = bee.transform.position;
                    //get the position of the player
                    Vector3 playerPosition = transform.position;
                    //get the distance between the two
                    float distance = Vector3.Distance(beePosition, playerPosition);
                    //check if the distance is less than the radius of the bee's collider
                    if (distance < 1.0f)
                    {
                        _gameDone = true;
                        Debug.Log("YOOO");
                        _manager.Lost();
                        break;
                        
                    }
                }
            }
        }

        // void OnCollisionEnter(Collision other) 
        // {
        //     Debug.Log("hey we lost");
        //     _manager.Lost();
        // }

        //make on trigger that prints hi:
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("hi");
        }

    }
}
