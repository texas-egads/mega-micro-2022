using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class BossAI : MonoBehaviour
    {

        int boss;
        public Animator anim;
        public GameObject moon;
        public GameObject leftLaser;
        public GameObject rightLaser;

        public GameObject axe;

        public GameObject felled;
        public GameObject bigRock;

        public GameObject godBack;
        public GameObject renBack;

        public AudioClip sound;
        AudioSource m;

        public bool notDead;



        private void Start()
        {
            m = Managers.AudioManager.CreateAudioSource();
            m.clip = sound;
            m.volume = 0.5f;

            notDead = true;
            boss = Random.Range(1, 3);
            if (boss == 1) //Rennala
            {
                Instantiate(renBack, new Vector3(-0.0019f, 0.26f, 0), Quaternion.identity);
                StartCoroutine(rennalaAttack());
            }
            else if (boss == 2) //Godrick
            {
                Instantiate(godBack, new Vector3(-0.0019f, 0.26f, 0), Quaternion.identity);
                anim.Play("GodrickIdle");
                anim.SetBool("Godrick", true);
                StartCoroutine(godrickAttack());
            }
        }

        IEnumerator godrickAttack()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++)
            {
                int attack = Random.Range(0, 3);
                switch (attack)
                {
                    case 0:
                        {
                            anim.SetBool("Godrick", false);
                            Instantiate(axe, new Vector3(-4.3f, 9.5f), Quaternion.identity);
                            yield return new WaitForSeconds(0.1f);
                            anim.SetBool("Godrick", true);
                            yield return new WaitForSeconds(2.9f);

                            break;
                        }
                    case 1:
                        {
                            anim.SetBool("Godrick", false);
                            Instantiate(axe, new Vector3(0, 9.5f), Quaternion.identity);
                            yield return new WaitForSeconds(0.1f);
                            anim.SetBool("Godrick", true);
                            yield return new WaitForSeconds(2.9f);
                            break;
                        }
                    case 2:
                        {
                            anim.SetBool("Godrick", false);
                            Instantiate(axe, new Vector3(4.3f, 9.5f), Quaternion.identity);
                            yield return new WaitForSeconds(0.1f);
                            anim.SetBool("Godrick", true);
                            yield return new WaitForSeconds(2.9f);
                            break;
                        }
                }
            }
            if (notDead)
            {
                m.Play();
                GameObject bRock = Instantiate(bigRock, new Vector3(-0.16f, 6), Quaternion.identity);
                if(boss == 2)
                {
                    bRock.GetComponent<Transform>().localScale *= 1.5f;
                }
                Instantiate(felled, new Vector3(0.03f, 0.15f, 0), Quaternion.identity);
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(2f);
            }
        }

        IEnumerator rennalaAttack()
        {
            bool canMoon = true;
            for(int i = 0; i < 3; i++)
            {
                int attack = Random.Range(0, 4);
                if (!canMoon && attack == 0)
                {
                    attack = Random.Range(1, 4);
                }
                switch (attack)
                {
                    case 0:
                        {
                            canMoon = false;
                            Instantiate(moon, new Vector3(0, 6), Quaternion.identity);
                            yield return new WaitForSeconds(4.2f);

                            break;
                        }
                    case 1:
                        {
                            //Laser Left
                            Instantiate(leftLaser, new Vector3(-7.45f, 2.25f), Quaternion.identity);
                            yield return new WaitForSeconds(1.5f);
                            break;
                        }
                    case 2:
                        {
                            //Laser Left
                            Instantiate(rightLaser, new Vector3(7.45f, 2.25f), Quaternion.identity);
                            yield return new WaitForSeconds(1.5f);
                            break;
                        }
                    case 3:
                        {
                            //Laser Sweep
                            Instantiate(rightLaser, new Vector3(7.45f, 2.25f), Quaternion.identity);
                            Instantiate(leftLaser, new Vector3(-7.45f, 2.25f), Quaternion.identity);
                            yield return new WaitForSeconds(1.5f);
                            break;
                        }
                }
            }
            if (notDead)
            {
                m.Play();
                Instantiate(bigRock, new Vector3(-0.16f, 6), Quaternion.identity);
                Instantiate(felled, new Vector3(0.03f, 0.15f, 0), Quaternion.identity);
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(2f);
            }
        }
    }
}
