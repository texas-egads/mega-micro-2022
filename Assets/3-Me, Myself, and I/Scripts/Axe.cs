using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class Axe : MonoBehaviour
    {
        bool goUp;
        public Rigidbody2D rb;
        public GameObject rumble;
        public AudioClip crunch;

        private void Start()
        {
            goUp = false;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            
            if (collision2D.gameObject.CompareTag("Ground"))
            {
                AudioSource m = Managers.AudioManager.CreateAudioSource();
                m.clip = crunch;
                m.volume = 0.2f;
                m.Play();
                goUp = true;
                StartCoroutine(activateTheRumbling());
            }
        }

        private void FixedUpdate()
        {
            if (goUp)
            {
                rb.gravityScale = 0;
                transform.position += new Vector3(0, 0.08f, 0);
            }
        }

        IEnumerator activateTheRumbling()
        {
            Instantiate(rumble, new Vector3(transform.position.x, -1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Instantiate(rumble, new Vector3(transform.position.x + 4.3f, -1f, 0), Quaternion.identity);
            GameObject rock = Instantiate(rumble, new Vector3(transform.position.x - 4.3f, -1f, 0), Quaternion.identity);
            rock.GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(0.5f);
            Instantiate(rumble, new Vector3(transform.position.x + 8.6f, -1f, 0), Quaternion.identity);
            rock = Instantiate(rumble, new Vector3(transform.position.x - 8.6f, -1f, 0), Quaternion.identity);
            rock.GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(0.3f);
        }
    }

}