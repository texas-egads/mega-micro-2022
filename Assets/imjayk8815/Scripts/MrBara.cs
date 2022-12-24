using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace imjayk8815
{
public class MrBara : MonoBehaviour
{
        [SerializeField] private Animator MrBaraAnimator;
        [SerializeField] private float health = 100;
        [SerializeField] private GameObject capybaraExplosion;
        [SerializeField] private float scaleMultiplier = 1f;
        [SerializeField] private CircleCollider2D[] circleColliders;
        [SerializeField] private AudioClip deathSound;
        private BoxCollider2D boxCollider;
        private SpriteRenderer sr;
        private bool hasWon;

        // Start is called before the first frame update
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            hasWon = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (health <= 0 && (hasWon == false))
            {
                sr.enabled = false;
                boxCollider.enabled = false;
                foreach(CircleCollider2D circle in circleColliders)
                {
                    circle.enabled = false;
                }
                //explode into smaller capybaras
                capybaraExplosion.SetActive(true);
                hasWon = true;
                Managers.AudioManager.CreateAudioSource().PlayOneShot(deathSound, .25f);
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Object 3")
            {
                transform.DOScale
                    (new Vector3(transform.localScale.x * scaleMultiplier, transform.localScale.y * scaleMultiplier, transform.localScale.z * scaleMultiplier), .3f).SetEase(Ease.InOutElastic);
                health -= 10;
                scaleMultiplier += .01f;
                print("health: " + health);
            }   
        }
    }
}
