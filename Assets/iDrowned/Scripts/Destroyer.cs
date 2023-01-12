using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace iDrowned{
public class Destroyer : MonoBehaviour
{


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(collision.gameObject);
        }
    }

}
