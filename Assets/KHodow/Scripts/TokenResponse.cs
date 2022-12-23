using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHodow
{
    public class TokenResponse : MonoBehaviour
    {
        [SerializeField] public GameObject tokenVariant;
        [SerializeField] public bool isFlipped;
        [SerializeField] private MatchingAndFlips matching; 
        [HideInInspector] private SpriteRenderer spriteRenderer;
        [HideInInspector] private GameObject spawnedFace;
        [HideInInspector] public bool isFlipping;
        [SerializeField] private bool isSelected;
        [SerializeField] private SpriteRenderer selectionCircle;
        [HideInInspector] public bool matched;

        public AudioClip cardFlipSound;
        
        private void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        private void Update()
        {
            if (!isFlipping && isSelected)
            {
                transform.localScale = new Vector3(0.825f, 0.825f, 1f);
            }
            else if (!isFlipping && !isSelected)
            {
                transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isSelected && !matched)
            {
                Flip();
            }
        }

        public void ManageSelect()
        {
            if (isSelected)
            {
                isSelected = false;
                selectionCircle.enabled = false;
            }
            else if (!isSelected)
            {
                isSelected = true;
                selectionCircle.enabled = true;
            }
        }

        private void Flip()
        {
            isFlipping = true;
            if (!isFlipped && matching.numFlips < 2)
            {
                PlayFlipSound();
                StartCoroutine(FlipTokenToFace());
                matching.numFlips++;
                return;
            }
            else if(isFlipped)
            {
                PlayFlipSound();
                StartCoroutine(FlipTokenToBack());
                matching.numFlips--;
            }
            else
            {
                isFlipping = false;
            }
        }

        private IEnumerator FlipTokenToFace()
        {
            selectionCircle.enabled = false;
            while(transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(transform.localScale.x-0.1f, transform.localScale.y, transform.localScale.z);
                yield return new WaitForSeconds(0.01f);
            }
            spriteRenderer.enabled = false;
            transform.localScale = new Vector3(0.75f, 0.75f, 1f);

            // Spawn Token Face
            spawnedFace = Instantiate(tokenVariant, transform.position, Quaternion.identity, transform);
            spawnedFace.transform.localScale = new Vector3(0f, 1f, 1f);
            while(spawnedFace.transform.localScale.x < 1f)
            {
                spawnedFace.transform.localScale = new Vector3(spawnedFace.transform.localScale.x+0.1f, spawnedFace.transform.localScale.y, spawnedFace.transform.localScale.z);
                yield return new WaitForSeconds(0.01f);
            }
            selectionCircle.enabled = true;
            isFlipping = false;
            isFlipped = true;
            matching.flippedTokens.Add(spawnedFace);
            yield return null;
        }
        private IEnumerator FlipTokenToBack()
        {
            selectionCircle.enabled = false;
            while(spawnedFace.transform.localScale.x > 0f)
            {
                spawnedFace.transform.localScale = new Vector3(spawnedFace.transform.localScale.x-0.1f, spawnedFace.transform.localScale.y, spawnedFace.transform.localScale.z);
                yield return new WaitForSeconds(0.01f);
            }
            matching.flippedTokens.Remove(spawnedFace);
            Destroy(spawnedFace);
            transform.localScale = new Vector3(0f, 0.825f, 1f);
            spriteRenderer.enabled = true;
            while(transform.localScale.x < 0.825f)
            {
                transform.localScale = new Vector3(transform.localScale.x+0.1f, transform.localScale.y, transform.localScale.z);
                yield return new WaitForSeconds(0.01f);
            }
            selectionCircle.enabled = true;
            isFlipped = false;
            isFlipping = false;
            yield return null;
        }

        private void PlayFlipSound()
        {
            AudioSource flip = Managers.AudioManager.CreateAudioSource();
            flip.loop = false;
            flip.volume = 0.75f;
            flip.clip = cardFlipSound;
            flip.Play();
        }
    }
}

