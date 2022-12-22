using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iDrowned
{
    public class ObjectShake : MonoBehaviour
    {
        [SerializeField] private float shakeMaxTimer, maxShakeAngle, maxShakeDistance;
        private Quaternion startRotation;
        private Vector3 startPosition;

        private void Awake()
        {
            startRotation = transform.rotation;
            startPosition = transform.position;
            Shake();
        }

        private void Shake()
        {
            float shakeAmount = maxShakeAngle-(Random.value * maxShakeAngle * 2);

            Vector3 shakeDistance = new Vector3(maxShakeDistance - (Random.value * maxShakeDistance * 2), maxShakeDistance - (Random.value * maxShakeDistance * 2),0);

            transform.rotation = startRotation;
            transform.Rotate(new Vector3(0, 0, shakeAmount));

            transform.position = startPosition;
            transform.Translate(shakeDistance);

            //float shakeTimer = Mathf.Max((Random.value * shakeMaxTimer),.1f);

            StartCoroutine(ShakeCoroutine(shakeMaxTimer));
        }

        IEnumerator ShakeCoroutine(float t)
        {
            yield return new WaitForSeconds(t);
            Shake();

        }
    }
}