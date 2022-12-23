using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MateriaHunter {
    public class ArrowMoverScript : MonoBehaviour
    {
        public float moveSpeed = 140;
        public float deadZone = 10;

        // Start is called before the first frame update
        void Start()
        {
            moveSpeed = moveSpeed / 60f;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

            if (transform.position.y > deadZone){
            //    Debug.Log("Arrow Deleted");
                Destroy(gameObject);
            }

        }
    }
}

