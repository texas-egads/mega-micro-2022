using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace iDrowned
{
    public class SpellGameManager : MonoBehaviour
    {

        private int word;
        [SerializeField] private string[] wordList = { "add", "ads", "dad", "sad", "saw", "wad", "was" };
        [SerializeField] private GameObject[] prefabs = new GameObject[7];
        private GameObject prefab;
        [SerializeField] private TMPro.TMP_Text letter1;
        [SerializeField] private TMPro.TMP_Text letter2;
        [SerializeField] private TMPro.TMP_Text letter3;
        [SerializeField] private TMPro.TMP_Text guess1;
        [SerializeField] private TMPro.TMP_Text guess2;
        [SerializeField] private TMPro.TMP_Text guess3;
        private TMPro.TMP_Text[] guessText = new TMPro.TMP_Text[3];
        private int current = 0;
        private IMinigamesManager minigame;
        private bool inputDisabled;
        [SerializeField] private float clearWordDelay = 0.5f;
        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject spellText;
        [SerializeField] private GameObject spellText2;
        [SerializeField] private BlinkGreen guessTextObject;
        [SerializeField] private GameObject dog;

        [SerializeField] private AudioClip winSound, gameLoop, placeSound, failSound;
        private IMinigameAudioManager audioManager;
        private AudioSource winAudio, gameAudio, placeAudio, failAudio;

        private void Awake()
        {
            minigame = Managers.MinigamesManager;

            audioManager = Managers.AudioManager;

            winAudio = audioManager.CreateAudioSource();
            gameAudio = audioManager.CreateAudioSource();
            placeAudio = audioManager.CreateAudioSource();
            failAudio = audioManager.CreateAudioSource();

            winAudio.clip = winSound;
            gameAudio.clip = gameLoop;
            placeAudio.clip = placeSound;
            failAudio.clip = failSound;


            word = Random.Range(0, wordList.Length);
            prefab = prefabs[word];

            letter1.text = "" + wordList[word][0];
            letter2.text = "" + wordList[word][1];
            letter3.text = "" + wordList[word][2];

            guessText[0] = guess1;
            guessText[1] = guess2;
            guessText[2] = guess3;
        }

        private void Start()
        {
            gameAudio.Play();

        }

        private void Update()
        {
            GetInputs();
        }


        private void GetInputs()
        {
            if (!inputDisabled && Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    CheckAnswer('W');
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    CheckAnswer('A');
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    CheckAnswer('S');
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    CheckAnswer('D');
                }
                else
                {
                    WrongAnswer();
                }
            }
        }

        private void CheckAnswer(char answer)
        {
            if (current <= 2 && answer == wordList[word][current])
            {
                CorrectAnswer();
                current++;
            }
            else WrongAnswer();
        }

        private void CorrectAnswer()
        {
            guessText[current].text = "" + wordList[word][current];
            placeAudio.Play();
            if (current >= 2)
            {
                WinGame();
                winAudio.Play();
            }
        }

        private void WrongAnswer()
        {
            guessText[current].text = "[]";
            failAudio.Play();
            StartCoroutine(ClearCoroutine());
            guess1.color = Color.red;
            guess2.color = Color.red;
            guess3.color = Color.red;
        }

        IEnumerator ClearCoroutine()
        {
            inputDisabled = true;
            yield return new WaitForSeconds(clearWordDelay);
            ClearWord();
        }

        private void ClearWord()
        {
            guess1.text = "_";
            guess2.text = "_";
            guess3.text = "_";
            guess1.color = Color.white;
            guess2.color = Color.white;
            guess3.color = Color.white;
            current = 0;
            inputDisabled = false;
        }

        private void WinGame()
        {
            inputDisabled = true;
            gameAudio.Stop();
            particles.SetActive(true);
            spellText.SetActive(false);
            spellText2.SetActive(true);
            letter1.gameObject.SetActive(false);
            letter2.gameObject.SetActive(false);
            letter3.gameObject.SetActive(false);
            guessTextObject.Blink();
            Instantiate(prefab, dog.transform.position, dog.transform.rotation);
            dog.SetActive(false);
            minigame.DeclareCurrentMinigameWon();
            minigame.EndCurrentMinigame(3.5f);
        }
    }

}
