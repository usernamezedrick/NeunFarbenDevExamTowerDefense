using System.Collections;
using UnityEngine;
using UniRx;
using NF.Main.Gameplay.Enemies;

namespace NF.Main.Gameplay.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] private GameObject[] enemyPrefabs; // Array of enemy types
        [SerializeField] private Transform spawnPoint;        // Spawn location for enemies
        [SerializeField] private Transform[] waypoints;         // Waypoints for enemy pathing

        [Header("Wave Settings")]
        [SerializeField] private int totalWaves = 5;
        [SerializeField] private int startEnemiesPerWave = 3;
        [SerializeField] private int wavesUntilNewEnemy = 3;  // Change enemy type every N waves
        [SerializeField] private float spawnDelay = 1f;         // Delay between enemy spawns

        private int currentWave = 0;
        private int enemiesRemaining = 0;

        private void Start()
        {
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            while (currentWave < totalWaves)
            {
                currentWave++;
                int enemiesToSpawn = startEnemiesPerWave + (currentWave - 1);
                enemiesRemaining = enemiesToSpawn;

                int enemyIndex = Mathf.Min(currentWave / wavesUntilNewEnemy, enemyPrefabs.Length - 1);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemy(enemyIndex);
                    yield return new WaitForSeconds(spawnDelay);
                }

                // Wait until all enemies from this wave are destroyed before starting the next wave.
                yield return new WaitUntil(() => enemiesRemaining <= 0);
            }

            Debug.Log("All waves completed!");
        }

        private void SpawnEnemy(int enemyIndex)
        {
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
            enemy.tag = "Enemy";

            // Get the EnemyController component (which should be attached to your enemy prefab).
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetWaypoints(waypoints);
                // Subscribe to the enemy's destruction event.
                enemyController.OnEnemyDestroyed
                    .Subscribe(_ => EnemyDestroyed())
                    .AddTo(enemy);
            }
            else
            {
                Debug.LogError("Enemy prefab does not have EnemyController attached!");
            }
        }

        private void EnemyDestroyed()
        {
            enemiesRemaining--;
            if (enemiesRemaining <= 0)
            {
                Debug.Log("Wave " + currentWave + " completed. Next wave will start.");
            }
        }
    }
}
