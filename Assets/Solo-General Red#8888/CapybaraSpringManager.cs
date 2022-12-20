using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Solo_General_Red_8888
{
    public class CapybaraSpringManager : MonoBehaviour
    {
        // Public Properties
        public List<CapybaraBucket> buckets = new List<CapybaraBucket>();
        
        // Private Properties
        [SerializeField] private float moveDuration = .25f;
        private int _currentBucketIndex;
        private int _bucketsFilled;
        private bool _hasOverfilled;

        private void OnEnable()
        {
            Wand.WandActivated += HandleWandActivated;
            Wand.WandMovedHorizontal += HandleWandMovedHorizontal;
            CapybaraBucket.BucketFilled += HandleBucketFilled;
            CapybaraBucket.BucketOverfilled += HandleBucketOverfilled;
        }

        private void OnDisable()
        {
            Wand.WandActivated -= HandleWandActivated;
            Wand.WandMovedHorizontal -= HandleWandMovedHorizontal;
            CapybaraBucket.BucketFilled -= HandleBucketFilled;
            CapybaraBucket.BucketOverfilled -= HandleBucketOverfilled;
        }


        private void MoveWandToCurrentBucket(Wand wand)
        {
            if (buckets.Count == 0 || buckets.Count <= _currentBucketIndex)
            {
                Debug.Log($"Invalid bucket index {_currentBucketIndex}");
                return;
            }
            
            CapybaraBucket currentBucket = buckets[_currentBucketIndex];
            
            Vector3 newTransform = wand.transform.position;
            newTransform.x = currentBucket.transform.position.x;

            wand.transform.DOMove(newTransform, moveDuration);
        }

        // Event callbacks
        public void HandleWandActivated()
        {
            if (buckets.Count == 0 || buckets.Count <= _currentBucketIndex)
            {
                Debug.Log($"Invalid bucket index {_currentBucketIndex}");
                return;
            }

            CapybaraBucket currentBucket = buckets[_currentBucketIndex];
            currentBucket.fillBucket();
        }

        public void HandleWandMovedHorizontal(Wand wand, bool isLeft)
        {
            if (isLeft)
            {
                if (_currentBucketIndex > 0)
                {
                    --_currentBucketIndex;
                    MoveWandToCurrentBucket(wand);
                }
            }
            else
            {
                if (_currentBucketIndex < buckets.Count)
                {
                    ++_currentBucketIndex;
                    MoveWandToCurrentBucket(wand);
                }
            }
        }

        public void HandleBucketFilled()
        {
            if (_hasOverfilled)
            {
                // Overfilled already, return
                return;
            }
            
            ++_bucketsFilled;

            if (_bucketsFilled == buckets.Count)
            {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
            }
        }

        public void HandleBucketOverfilled()
        {
            _hasOverfilled = true;
            Managers.MinigamesManager.DeclareCurrentMinigameLost();
            Managers.MinigamesManager.EndCurrentMinigame();  // Maybe do this after animation
        }
    }
}
