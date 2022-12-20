using System;
using UnityEngine;

namespace Solo_General_Red_8888
{
    public class Wand : MonoBehaviour
    {
        // Tunable Control Properties
        [SerializeField] private float horizontalDeadZone = .1f;
        
        // Public Properties
        
        
        // Events
        public static event Action WandActivated;
        public static event Action<Wand, bool> WandMovedHorizontal;  // params: Wand wand, bool isLeft

        // Update is called once per frame
        void Update()
        {
            float horizontalValue = Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalValue) > horizontalDeadZone)
            {
                HandleHorizontal(horizontalValue);
            }

            if (Input.GetButtonDown("Space"))
            {
                WandActivated?.Invoke();
            }
        }
        
        // Handle Horizontal movement
        void HandleHorizontal(float axisValue)
        {
            bool isLeft = axisValue < 0;
            WandMovedHorizontal?.Invoke(this, isLeft);
        }

    }
}
