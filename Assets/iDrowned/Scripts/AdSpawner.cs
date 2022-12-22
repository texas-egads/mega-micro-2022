using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iDrowned
{
public class AdSpawner : MonoBehaviour
{
    [SerializeField] private float delay = .2f;

    [SerializeField] private GameObject[] prefabs;

    // Start is called before the first frame update
    void Start()
        {
            StartCoroutine(spawnAd());
        }

    IEnumerator spawnAd()
    {
            int c = 0;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                Instantiate(prefabs[c], new Vector3(8 - (Random.value * 16), 4 - (Random.value * 8), 0), transform.rotation);
                c++;
                if (c>2)
                {
                    c = 0;
                }
            }
        }
}

}
