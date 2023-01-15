using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MateriaHunter {
    public class RightArrowMoveScript : MonoBehaviour
    {
        public float moveSpeed = 3;
        public float deadZone = 10;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = transform.position + (Vector3.up * moveSpeed) * Time.deltaTime;

            if (transform.position.y > deadZone){
                Debug.Log("Arrow Deleted");
                Destroy(gameObject);
            }

        }
    }
}
