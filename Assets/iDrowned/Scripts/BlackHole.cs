using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace iDrowned
{
public class BlackHole : MonoBehaviour
{

        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float mass = 1f;
        [SerializeField] private List<Rigidbody2D> bodies = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
            bodies.Add(GetComponent<Rigidbody2D>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            foreach (Rigidbody2D body in bodies)
            {
                Vector3 direction = transform.position - body.transform.position;
                float distance = direction.magnitude;
                if (distance > .001f)
                {
                float force = gravity * (mass * body.mass) / Mathf.Pow(distance, 2);
                body.AddForce(direction.normalized * force);

                }

            }

    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                bodies.Add(rb);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                bodies.Remove(rb);
            }
        }
    }

}
