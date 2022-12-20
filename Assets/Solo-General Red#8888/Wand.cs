using System;
using UnityEngine;

namespace Solo_General_Red_8888
{
    public class Wand : MonoBehaviour
    {
        // Events
        public static event Action WandActivated;
        public static event Action<Wand, bool> WandMovedHorizontal;  // params: Wand wand, bool isLeft

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                HandleHorizontal(Input.GetAxis("Horizontal"));
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
