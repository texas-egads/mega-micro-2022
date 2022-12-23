using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
    public class kill_player : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            Debug.Log("Player is dead");
        }
    }
}
