using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrFlambo
{

    public class Rumbling : MonoBehaviour
    {

        public GameObject particle;
        public GameObject death;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(boom());
        }

        IEnumerator boom()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject rock = Instantiate(particle, transform.position, Quaternion.identity);
            Instantiate(death, transform.position - new Vector3(0, 1.5f, 0), Quaternion.identity);
            if (GetComponent<SpriteRenderer>().flipX)
            {
                rock.GetComponent<SpriteRenderer>().flipX = true;
            }
            Destroy(gameObject);
        }

    }
}