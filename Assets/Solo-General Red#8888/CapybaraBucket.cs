using System;
using System.Collections.Generic;
using UnityEngine;

namespace Solo_General_Red_8888
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CapybaraBucket : MonoBehaviour
    {
        // Events
        public static event Action BucketFilled;
        public static event Action<float> BucketOverfilled;

        // Public Properties
        public AudioClip transformNeutralClip;
        public AudioClip transformSuccessClip;
        public AudioClip transformFailedClip;
        public SpriteRenderer spriteRenderer;
        
        // Private properties
        [SerializeField] private int maxFillLevel = 3;
        [SerializeField] private List<Sprite> transformations;
        private int _fillLevel;
        private AudioSource _bucketAudioSource;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = transformations[0];
            _bucketAudioSource = Managers.AudioManager.CreateAudioSource();
        }

        public void FillBucket()
        {
            ++_fillLevel;
            if (_fillLevel < maxFillLevel)
            {
                spriteRenderer.sprite = transformations[_fillLevel];
                _bucketAudioSource.clip = transformNeutralClip;
                _bucketAudioSource.Play();
            }
            else if (_fillLevel == maxFillLevel)
            {
                spriteRenderer.sprite = transformations[_fillLevel];
                _bucketAudioSource.clip = transformSuccessClip;
                _bucketAudioSource.Play();
                BucketFilled?.Invoke();
            }
            else
            {
                spriteRenderer.sprite = null;
                _bucketAudioSource.clip = transformFailedClip;
                _bucketAudioSource.Play();
                BucketOverfilled?.Invoke(transformSuccessClip.length);
            }
        }
    }
}
