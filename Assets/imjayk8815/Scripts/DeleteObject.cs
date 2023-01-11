using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace imjayk8815
{
    public class DeleteObject : MonoBehaviour
    {
        [SerializeField] private float secondsTillDestroyed = .5f;

        private void OnEnable() {
            StartCoroutine(DeleteParticle());
        }

        private IEnumerator DeleteParticle()
        {
            yield return new WaitForSeconds(secondsTillDestroyed);
            Destroy(gameObject);
        }
    }
}
