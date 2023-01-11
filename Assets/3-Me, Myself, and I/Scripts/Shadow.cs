using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class Shadow : MonoBehaviour
    {
        public Transform player;

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(player.position.x, -2.8f, 0f);
        }
    }
}
