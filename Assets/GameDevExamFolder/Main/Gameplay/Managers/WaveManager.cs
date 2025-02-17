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
        [SerializeField] private Transform spawnPoint;      // Spawn location for enemies
        [SerializeField] private Transform[] waypoints;     // Waypoints for enemy pathing

        [Header("Wave Settings")]
        [SerializeField] private int totalWaves = 5;
        [SerializeField] private int startEnemiesPerWave = 3;
        [SerializeField] private int wavesUntilNewEnemy = 3;
        [SerializeField] private float spawnDelay = 1f;

        private int currentWave = 0;
        private int enemiesRemaining = 0;
        private bool isWaveStarted = false; // Prevents waves from starting automatically

        private void Start()
        {
            Debug.Log("WaveManager: Ready, but waiting for StartWaves() to be called.");
        }

        public void StartWaves() // Called by LevelStartController when Start button is pressed
        {
            if (!isWaveStarted)
            {
                isWaveStarted = true;
                Debug.Log("WaveManager: StartWaves() called. Starting wave spawning...");
                StartCoroutine(SpawnWaves());
            }
            else
            {
                Debug.LogWarning("WaveManager: StartWaves() was called again, but waves have already started.");
            }
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitUntil(() => Time.timeScale > 0);
            Debug.Log("WaveManager: SpawnWaves() started.");

            while (currentWave < totalWaves)
            {
                currentWave++;
                Debug.Log("WaveManager: Starting wave " + currentWave);

                int enemiesToSpawn = startEnemiesPerWave + (currentWave - 1);
                enemiesRemaining = enemiesToSpawn;

                int enemyIndex = Mathf.Min(currentWave / wavesUntilNewEnemy, enemyPrefabs.Length - 1);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Debug.Log("WaveManager: Spawning enemy " + (i + 1) + " of " + enemiesToSpawn);
                    SpawnEnemy(enemyIndex);
                    yield return new WaitForSeconds(spawnDelay);
                }

                yield return new WaitUntil(() => enemiesRemaining <= 0);
                Debug.Log("WaveManager: Wave " + currentWave + " completed.");
            }

            Debug.Log("WaveManager: All waves completed!");
        }

        private void SpawnEnemy(int enemyIndex)
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("WaveManager: No enemy prefabs assigned! Check the Inspector.");
                return;
            }

            if (enemyIndex < 0 || enemyIndex >= enemyPrefabs.Length)
            {
                Debug.LogError("WaveManager: Invalid enemy index " + enemyIndex + ". Check wave settings.");
                return;
            }

            if (spawnPoint == null)
            {
                Debug.LogError("WaveManager: spawnPoint is not assigned in the Inspector!");
                return;
            }

            GameObject enemyPrefab = enemyPrefabs[enemyIndex];
            if (enemyPrefab == null)
            {
                Debug.LogError("WaveManager: Enemy prefab at index " + enemyIndex + " is null!");
                return;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("WaveManager: Spawned enemy at " + spawnPoint.position);

            enemy.tag = "Enemy";

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetWaypoints(waypoints);
                enemyController.OnEnemyDestroyed.Subscribe(_ => EnemyDestroyed()).AddTo(enemy);
            }
            else
            {
                Debug.LogError("WaveManager: Enemy prefab does not have an EnemyController script!");
            }
        }

        private void EnemyDestroyed()
        {
            enemiesRemaining--;
            if (enemiesRemaining <= 0)
            {
                Debug.Log("WaveManager: Wave " + currentWave + " completed. Next wave will start.");
            }
        }
    }
}
