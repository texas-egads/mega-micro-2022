using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class Capybara : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        private Rigidbody2D rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }


        void FixedUpdate()
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            move = move.normalized * speed;
            rb.velocity = move;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.name.Contains("Enemy"))
            {
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(1f);
            }
        }
    }
}