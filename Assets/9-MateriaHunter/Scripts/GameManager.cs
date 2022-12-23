using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MateriaHunter {

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public int currentScore;
        public int scorePerNote = 1;
        public GameObject hitEffect;
        public GameObject resultScreen;
        public GameObject tryAgainScreen;
        public Text scoreText;
        public int counter;
 
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
         
        }

        // Update is called once per frame
        void Update()
        {
            if (currentScore == 5 && !resultScreen.activeInHierarchy) {

                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                resultScreen.SetActive(true);
            }

            if (counter == 5 && !(currentScore == 5)) {
                tryAgainScreen.SetActive(true);
            }
            

        }

        [ContextMenu("Increase Score")]
        public void NoteHit() {
           // Debug.Log("hit note");
            currentScore += scorePerNote;
            scoreText.text = currentScore.ToString();
            SpawnOrange();
            counter += 1;
        }
        public void NoteMissed() {
         //   Debug.Log("Missed note");
            counter += 1;
        }

        public void SpawnOrange() {
            Instantiate(hitEffect, transform.position, transform.rotation);
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0f);
        }   
    }


   
}