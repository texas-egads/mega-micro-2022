using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MateriaHunter {
    public class ArrowGeneratorScript : MonoBehaviour
    {
        public GameObject objectToSpawn;

        public float spawnRate = 2;
        private float timer = 0;
        // Start is called before the first frame update
        void Start()
        {
            SpawnObject();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTimer();
        }

    private void UpdateTimer() {
        if(timer < spawnRate ) {
                timer += Time.deltaTime;

            } else {
                SpawnObject();
                timer = 0;
            }
    }

    public void SpawnObject() {
        Instantiate(objectToSpawn, transform.position, transform.rotation);

    }

    }
}