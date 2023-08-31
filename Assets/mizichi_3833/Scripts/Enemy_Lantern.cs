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

        GameObject killerWall;

        private Animator anim;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        void Update()
        {
            StartCoroutine(WaitBeforeTurn(Random.Range(2f,9f)));//random time for the character to turn

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
                    Debug.DrawLine(castPoint.position, hit.point, Color.red);


                }
                else
                {
                    val = false;
                }
            }
            else
            {
                Debug.DrawLine(castPoint.position, endPos, Color.yellow);
                val = false;

            }

            return val;
        }

        IEnumerator WaitBeforeTurn(float time)
        {
            yield return new WaitForSeconds(time);
            anim.SetTrigger("look");

            if (CanSeePlayer(enemyRange))
            {
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                yield return new WaitForSeconds(1);
                Managers.MinigamesManager.EndCurrentMinigame(1);
            }
            else
            {

                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                yield return new WaitForSeconds(1);
                Managers.MinigamesManager.EndCurrentMinigame(1);

            }
        }

    }
}


