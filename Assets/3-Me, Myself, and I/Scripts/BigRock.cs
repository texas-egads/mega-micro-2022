using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class BigRock : MonoBehaviour
    {
        bool fallen;
        public GameObject particulates;
        private void Start()
        {
            fallen = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y > 0.2f)
            {
                transform.position += new Vector3(0, -0.1f, 0);
            }
            else if(transform.position.y <= 0.2f)
            {
                transform.position = new Vector3(transform.position.x, 0.2f);
                if (!fallen)
                {
                    Instantiate(particulates, transform.position - new Vector3(0, -0,1f), Quaternion.identity);
                    fallen = true;
                }
            }
        }
    }
}