using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHodow
{
    public class MatchingAndFlips : MonoBehaviour
    {
        public int numMatches;
        public int numFlips;
        private bool isMatch;

        public List<GameObject> flippedTokens;
        [SerializeField] private GameObject confetti;
        [SerializeField] private GameObject winCanvas;


        public AudioClip winSound;

        private void Update() {
            if (isMatch)
            {
                return;
            }
            if (flippedTokens.Count == 2 && flippedTokens[0].name == flippedTokens[1].name)
            {
                isMatch = true;
                ManageMatch();
            }
            if (numMatches == 1)
            {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(3f);
                confetti.SetActive(true);
                winCanvas.SetActive(true);
                isMatch = true;

                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.loop = false;
                win.volume = 0.5f;
                win.clip = winSound;
                win.Play();
            }
        }

        private void ManageMatch()
        {
            numMatches++;
            numFlips = 0;
            foreach(GameObject token in flippedTokens)
            {
                TokenResponse tokenBack = Physics2D.OverlapCircle(token.transform.position, 0.01f).GetComponent<TokenResponse>();
                tokenBack.matched = true;
            }
            flippedTokens = new List<GameObject>();
            isMatch = false;
        }
    }
}

