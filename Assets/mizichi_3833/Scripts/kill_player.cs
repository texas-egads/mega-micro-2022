using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
    public class kill_player : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Object 4")
            {
                Debug.Log("OutOfBound");
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(0);
            }
        }
    }
}
