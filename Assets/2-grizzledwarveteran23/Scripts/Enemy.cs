using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class Enemy : MonoBehaviour
    {
        public float speed;
        public float lifeTime;

        void Start() {
            StartCoroutine(Die(lifeTime));
        }
        
        void FixedUpdate() {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        IEnumerator Die(float time) {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
