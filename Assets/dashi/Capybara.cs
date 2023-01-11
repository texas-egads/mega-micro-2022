using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dashi
{
    public class Capybara : MonoBehaviour
    {
        [SerializeField] private dashi.GameManager _manager;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void OnCollisionEnter(Collision other) 
        {
            _manager.Lost();
        }
    }
}
