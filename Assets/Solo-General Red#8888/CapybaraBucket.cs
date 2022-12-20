using System;
using UnityEngine;

namespace Solo_General_Red_8888
{
    public class CapybaraBucket : MonoBehaviour
    {
        // Events
        public static event Action BucketFilled;
        public static event Action BucketOverfilled;

        // Private properties
        [SerializeField] private int maxFillLevel = 3;
        private int _fillLevel;

        public void fillBucket()
        {
            ++_fillLevel;
            if (_fillLevel == maxFillLevel)
            {
                BucketFilled?.Invoke();
            }
            else if (_fillLevel > maxFillLevel)
            {
                BucketOverfilled?.Invoke();
            }
        }
    }
}
