using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wombies
{
    public class HatReveal : MonoBehaviour
    {

        Vector2 newPosition;
        private float moveYAmount = 1.5f;

    // Start is called before the first frame update
        void Start()
        {
            // getting position of current hat 
            newPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

