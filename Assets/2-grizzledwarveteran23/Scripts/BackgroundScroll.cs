using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField]
        private float scrollSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        }
    }
}
