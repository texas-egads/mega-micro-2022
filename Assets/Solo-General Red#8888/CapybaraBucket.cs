using System;
using UnityEngine;

namespace Solo_General_Red_8888
{
    public class CapybaraBucket : MonoBehaviour
    {
        // Events
        public static event Action BucketFilled;
        public static event Action BucketOverfilled;

        // Public Properties
        public AudioClip transformNeutralClip;
        public AudioClip transformSuccessClip;
        public AudioClip transformFailedClip;
        
        // Private properties
        [SerializeField] private int maxFillLevel = 3;
        private int _fillLevel;
        private AudioSource _bucketAudioSource;

        private void Start()
        {
            _bucketAudioSource = Managers.AudioManager.CreateAudioSource();
        }

        public void FillBucket()
        {
            ++_fillLevel;
            if (_fillLevel < maxFillLevel)
            {
                _bucketAudioSource.clip = transformNeutralClip;
                _bucketAudioSource.Play();
            }
            else if (_fillLevel == maxFillLevel)
            {
                _bucketAudioSource.clip = transformSuccessClip;
                _bucketAudioSource.Play();
                BucketFilled?.Invoke();
            }
            else if (_fillLevel == maxFillLevel + 1)
            {
                _bucketAudioSource.clip = transformFailedClip;
                _bucketAudioSource.Play();
                Invoke("PlayBucketOverfilled", transformSuccessClip.length);
            }
        }

        void PlayBucketOverfilled()
        {
            BucketOverfilled?.Invoke();
        }
    }
}
