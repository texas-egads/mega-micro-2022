using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
    public class Enemy_Lantern : MonoBehaviour
    {
        [SerializeField]
        Transform player;
        [SerializeField]
        Transform castPoint;
        [SerializeField]
        float enemyRange;

        bool isFacingLeft;

        void Update()
        {
            //float disToPlayer = Vector2.Distance(transform.position, player.position);

            if (CanSeePlayer(enemyRange))
            {
                //player is caught
                //Managers.MinigamesManager.DeclareCurrentMinigameLost();

            }
            else
            {
                //player is safe
            }
        }

        bool CanSeePlayer(float range)
        {
            bool val = false;
            Vector2 endPos = castPoint.position + Vector3.right * -range;

            RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    val = true;
                }
                else
                {
                    val = false;
                }
                Debug.DrawLine(castPoint.position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(castPoint.position, endPos, Color.yellow);

            }

            return val;
        }
    }
}

//Managers.MinigamesManager.DeclareCurrentMinigameWon();


