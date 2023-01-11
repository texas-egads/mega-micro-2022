using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{
    public class LaserLeft : MonoBehaviour
    {
        public float death;
        public float speed;
        public AudioClip buzz;

        // Start is called before the first frame update

        private void Start()
        {
            AudioSource m = Managers.AudioManager.CreateAudioSource();
            m.clip = buzz;
            m.Play();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (death <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                death -= 0.1f;
            }

            transform.position += new Vector3(speed, 0f, 0f);
        }
    }
}
