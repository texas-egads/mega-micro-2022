using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace imjayk8815
{
    public class Wizard : MonoBehaviour
    {
        [SerializeField] private Transform firePosition;
        [SerializeField] private GameObject fireBall;
        [SerializeField] private GameObject spellCast;
        [SerializeField] private AudioClip castSound;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spellCast.SetActive(true);
                Instantiate(fireBall, firePosition.position, Quaternion.identity);
                Invoke("TurnOffParticle", .3f);
                Managers.AudioManager.CreateAudioSource().PlayOneShot(castSound, .25f);
            }
        }

        private void TurnOffParticle()
        {
            spellCast.SetActive(false);
        }
    }
}
