using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


namespace kelly
{
    public class Summon : MonoBehaviour
    {

        [SerializeField]
        List<GameObject> arrowList = new List<GameObject>();

        [SerializeField]
        List<Sprite> arrowSprites = new List<Sprite>();

        [SerializeField]
        List<Sprite> arrowUnlitSprites = new List<Sprite>();

        [SerializeField]
        int numArrows = 5;

        [SerializeField]
        Transform arrowTransform;
        float initialY;

        [SerializeField]
        Sprite[] handSprites;

        [SerializeField]
        SpriteRenderer handSpriteRenderer;

        [SerializeField]
        GameObject capybara;

        [SerializeField]
        ParticleSystem particle = null;

        [SerializeField]
        ParticleSystem particleCast = null;

        [SerializeField]
        float speed = 10f;
        [SerializeField]
        float magnitude = .01f;

        [SerializeField]
        private float shakeTimer = 0.0f;
        private float shakeEnd = 0.5f;

        public AudioClip castSound;
        public AudioClip failSound;
        public AudioClip winSound;

        private List<GameObject> genArrows;
        private List<int> expected;
        int cur = 0;

        IDictionary<string, int> inputDict = new Dictionary<string, int>() { { "w", 0 }, { "a", 1 }, { "s", 2 }, { "d", 3 } };

        // Start is called before the first frame update
        void Start()
        {
            cur = 0;
            genArrows = new List<GameObject>();
            expected = new List<int>();

            initialY = arrowTransform.position.y;
            // generate random directions
            for (int i = 0; i < numArrows; i++)
            {
                int direction = Random.Range(0, arrowList.Count);
                GameObject arrow = Instantiate(arrowList[direction]) as GameObject;
                arrow.transform.position = arrowTransform.position + new Vector3(0, -1.2f * i, 0);
                genArrows.Add(arrow);
                arrow.transform.parent = arrowTransform;
                arrow.GetComponent<SpriteRenderer>().sprite = arrowUnlitSprites[direction];
                expected.Add(direction);
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown("w"))
            {
                doMatch("w");
                handSpriteRenderer.sprite = handSprites[0];
            }
            if (Input.GetKeyDown("a"))
            {
                doMatch("a");
                handSpriteRenderer.sprite = handSprites[1];

            }
            if (Input.GetKeyDown("s"))
            {
                doMatch("s");
                handSpriteRenderer.sprite = handSprites[2];

            }
            if (Input.GetKeyDown("d"))
            {
                doMatch("d");
                handSpriteRenderer.sprite = handSprites[3];

            }
            if (shakeTimer > 0)
            {
                arrowTransform.position = new Vector3(arrowTransform.transform.position.x, initialY + Mathf.Sin(Time.time * speed) * magnitude, arrowTransform.transform.position.z);
                shakeTimer -= Time.deltaTime;
            }
        }

        private bool doMatch(string input)
        {
            //Debug.Log(input);

            if (expected[cur] == inputDict[input])
            {
                particleCast.Play();
                playSpellSound();
                if (cur < genArrows.Count)
                {
                    genArrows[cur].GetComponent<SpriteRenderer>().sprite = arrowSprites[expected[cur]];
                }
                cur++;
                if (cur == expected.Count)
                {
                    handleWin();
                }
                return true;
            }
            else
            {
                for (int i = 0; i < cur; i++)
                {
                    genArrows[i].GetComponent<SpriteRenderer>().sprite = arrowUnlitSprites[expected[i]];
                }
                playFailSound();
                cur = 0;
                shakeTimer = shakeEnd;
                return false;
            }
        }

        private void playSpellSound()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.pitch = (Random.Range(0.9f, 2f));
            audioSource.clip = castSound;
            audioSource.Play();
        }

        private void playFailSound()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.pitch = (Random.Range(0.9f, 1f));
            audioSource.clip = failSound;
            audioSource.Play();
        }

        private void handleWin()
        {
            shakeTimer = .2f;

            particle.Play();
            capybara.SetActive(true);
            AudioSource win = Managers.AudioManager.CreateAudioSource();
            win.PlayOneShot(winSound);
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(2f);
        }
    }

}