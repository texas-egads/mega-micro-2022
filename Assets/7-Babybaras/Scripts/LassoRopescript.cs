using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Babybaras
{
    public class LassoRopescript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 difference = new Vector3(GameObject.FindGameObjectsWithTag("GameController")[0].transform.position.x - transform.position.x, GameObject.FindGameObjectsWithTag("GameController")[0].transform.position.y - transform.position.y, 0);
            Vector3 scaleVec = new Vector3(GetComponent<SpriteRenderer>().bounds.size.x, difference.y / GetComponent<SpriteRenderer>().bounds.size.y, 0);
            Debug.Log(GetComponent<SpriteRenderer>().bounds.size);
            Debug.Log(transform.position.x);
            transform.localScale = scaleVec;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Rad2Deg* Lassoscript.getLassoAngle());
            
        }
    }

}

