using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class Enemy : MonoBehaviour
    {
        public float speed;
        
        void FixedUpdate() {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}
