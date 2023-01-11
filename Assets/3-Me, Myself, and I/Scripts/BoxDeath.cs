using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class BoxDeath : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(die());
        }

        IEnumerator die()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}