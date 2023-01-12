using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loremonger
{
    public class Letter : MonoBehaviour
    {
        public GameObject version2d, version3d;


        public void Correct()
        {
            version2d.SetActive(false);
            version3d.SetActive(true);
        }

        public void Reset()
        {
            version2d.SetActive(true);
            version3d.SetActive(false);
        }


    }
}

