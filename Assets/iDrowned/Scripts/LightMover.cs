using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace iDrowned
{


    public class LightMover : MonoBehaviour
    {

        [SerializeField] private float frequency = 1f; // frequency of the sine wave, determines the speed of the rotation
        [SerializeField] private float amplitude = 30f; // amplitude of the sine wave, determines the range of the rotation

        private float startAngle; // starting angle of the object
        private float endAngle; // ending angle of the object

        void Start()
        {
            // set the starting and ending angles for the object
            startAngle = transform.localEulerAngles.z - amplitude;
            endAngle = transform.localEulerAngles.z + amplitude;
        }

        void Update()
        {
            // use a sine wave to drive the rotation of the object
            float rotation = Mathf.Sin(Time.time * frequency) * amplitude;

            // set the object's rotation to the sine wave value, clamped between the start and end angles
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(rotation, startAngle, endAngle));
        }
    }

}