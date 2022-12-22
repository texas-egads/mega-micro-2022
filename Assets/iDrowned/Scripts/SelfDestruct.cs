using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iDrowned
{
public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 6);
    }


}

}
