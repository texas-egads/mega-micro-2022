using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace imjayk8815
{
    public class UI_Script : MonoBehaviour
    {
        [SerializeField] Sprite ui_space_Up;
        [SerializeField] Sprite ui_space_Down;
        private SpriteRenderer sr;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                sr.sprite = ui_space_Up;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                sr.sprite = ui_space_Down;
            }
        }
    }
}