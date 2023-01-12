using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Loremonger
{
    public class SpellCaster : MonoBehaviour
    {
        public Transform[] waypoints;
        public GameObject wPrefab, aPrefab, sPrefab, dPrefab;
        public GameObject magicat;
        public GameObject capybara;
        public GameObject poof;
        public GameObject clouds;
        public GameObject audience;
        public GameObject confetti;
        public AudioClip bgm;
        public AudioClip ding;
        public AudioClip woohoo;
        public AudioClip buzzer;
        public AudioClip cheer;
        private int spellLength = 10;
        private string spell;
        private char[] wasd = { 'W', 'A', 'S', 'D' };
        private string currentSpell = "";

        // Start is called before the first frame update
        void Start()
        {
            spell = Spell(spellLength);

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = bgm;
            loop.Play();

        }

        // Update is called once per frame
        void Update()
        {
            AttemptSpell();
        }

        string Spell(int spellLength)
        {
            string spell = "";

            for (int i = 0; i < spellLength; i++)
            {
                string letter = wasd[Random.Range(0, 4)].ToString();
                spell += letter;

                GameObject piece = null;
                switch (letter)
                {
                    case ("W"):
                        piece = Instantiate(wPrefab, waypoints[i].position, Quaternion.identity);
                        break;
                    case ("A"):
                        piece = Instantiate(aPrefab, waypoints[i].position, Quaternion.identity);
                        break;
                    case ("S"):
                        piece = Instantiate(sPrefab, waypoints[i].position, Quaternion.identity);
                        break;
                    case ("D"):
                        piece = Instantiate(dPrefab, waypoints[i].position, Quaternion.identity);
                        break;
                }

                piece.transform.parent = waypoints[i].transform;
            }

            return spell;
        }

        void AttemptSpell()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.W)) { currentSpell += "W"; magicat.GetComponent<Animator>().SetTrigger("W"); }
                if (Input.GetKeyDown(KeyCode.A)) { currentSpell += "A"; magicat.GetComponent<Animator>().SetTrigger("A"); }
                if (Input.GetKeyDown(KeyCode.S)) { currentSpell += "S"; magicat.GetComponent<Animator>().SetTrigger("S"); }
                if (Input.GetKeyDown(KeyCode.D)) { currentSpell += "D"; magicat.GetComponent<Animator>().SetTrigger("D"); }

                if (currentSpell == spell.Substring(0, currentSpell.Length) && currentSpell != "")
                {
                    //Debug.Log(currentSpell + " matches " + spell.Substring(0, currentSpell.Length));
                    waypoints[currentSpell.Length - 1].transform.GetComponentInChildren<Letter>().Correct();
                    AudioSource correct = Managers.AudioManager.CreateAudioSource();
                    correct.PlayOneShot(ding);

                    if (currentSpell == spell)
                    {
                        //Debug.Log("Spell complete!");
                        Managers.MinigamesManager.DeclareCurrentMinigameWon();
                        magicat.GetComponent<Animator>().SetBool("Win", true);
                        clouds.GetComponent<Animator>().SetBool("Win", true);
                        audience.GetComponent<Animator>().SetBool("Win", true);
                        confetti.SetActive(true);
                        poof.SetActive(true);
                        capybara.SetActive(true);
                        AudioSource win = Managers.AudioManager.CreateAudioSource();
                        win.PlayOneShot(woohoo);
                        AudioSource crowd = Managers.AudioManager.CreateAudioSource();
                        crowd.PlayOneShot(cheer);
                    }
                }
                else
                {
                    //Debug.Log("Wrong!");
                    magicat.GetComponent<Animator>().SetTrigger("Wrong");
                    AudioSource wrong = Managers.AudioManager.CreateAudioSource();
                    wrong.PlayOneShot(buzzer);
                    for (int i = 0; i < currentSpell.Length; i++)
                    {
                        waypoints[i].transform.GetComponentInChildren<Letter>().Reset();
                    }

                    currentSpell = "";
                }
            }
            
        }
    }
}


