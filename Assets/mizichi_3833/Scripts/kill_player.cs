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
                Destroy(other);
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(0);
            }
        }
    }
}
