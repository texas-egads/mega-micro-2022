using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grizzledwarveteran23 {
    public class EnemySpawner : MonoBehaviour
    {
        //Enemies will randomly spawn at a predetermined set of points
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private Transform[] spawnPoints = new Transform[3];

        [SerializeField]
        private float spawnRate;

        private bool isSpawning = false;

        // Update is called once per frame
        void Update()
        {
            if(!isSpawning)
            {
            StartCoroutine(SpawnEnemies());
            }
        }

        IEnumerator SpawnEnemies()
        {
            isSpawning = true;
            yield return new WaitForSeconds(spawnRate);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            isSpawning = false;
        }
    }
}
