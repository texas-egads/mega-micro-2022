using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class Rotate : MonoBehaviour
    {
        public float speed;

        private void FixedUpdate()
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z + speed));
        }
    }
}
