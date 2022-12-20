using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIZICHI
{
    public class Enemy_Lantern : MonoBehaviour
    {
        [SerializeField]
        Transform castPoint;
        [SerializeField]
        float enemyRange;

        void Update()
        {
            StartCoroutine(WaitBeforeTurn(Random.Range(2f,6f)));//random time for the character to turn

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

        IEnumerator WaitBeforeTurn(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Tranform!");
            yield return new WaitForSeconds(1);
            //activate animation transition
            if (CanSeePlayer(enemyRange))
            {
                Debug.Log("Caught");
                //player is caught
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(1);

            }
            else
            {
                Debug.Log("safe");
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                enemyRange = -enemyRange;
                //player is safe
            }
        }
    }
}


