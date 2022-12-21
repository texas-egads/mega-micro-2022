using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class EnemySpawner : MonoBehaviour
    {
        //Enemies will randomly spawn at a predetermined set of points
        [SerializeField]
        private GameObject[] enemyPrefabs;
        [SerializeField]
        private Transform[] spawnPoints = new Transform[6];

        [SerializeField]
        private float spawnRate;

        private bool isSpawning = false;

        int counter = 0;

        int prefabIndex = 0;

        // Update is called once per frame
        void Update()
        {
            if(!isSpawning && counter < 13)
            {
                counter++;
                StartCoroutine(SpawnEnemies());
            }
        }

        IEnumerator SpawnEnemies()
        {
            isSpawning = true;
            yield return new WaitForSeconds(spawnRate);
            int choice = Random.Range(0, 10);
            int spawnPointIndex;
            if(choice <= 7) {
                 spawnPointIndex = Random.Range(0, 3);
            } else {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            }
            GameObject instance = Instantiate(enemyPrefabs[prefabIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            if(spawnPointIndex >= 3) {
                instance.GetComponent<Enemy>().speed = 6;
            }
            spawnRate = Mathf.Max(0.55f, spawnRate - 0.1f);
            prefabIndex = (prefabIndex + 1) % enemyPrefabs.Length;
            isSpawning = false;
        }
    }
}
