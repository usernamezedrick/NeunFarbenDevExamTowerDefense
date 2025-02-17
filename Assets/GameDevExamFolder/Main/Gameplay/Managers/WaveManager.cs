using System.Collections;
using UnityEngine;
using UniRx;
using NF.Main.Gameplay.Enemies;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform[] waypoints;

        [Header("Wave Settings")]
        [SerializeField] private int totalWaves = 5;
        [SerializeField] private int startEnemiesPerWave = 3;
        [SerializeField] private int wavesUntilNewEnemy = 3;
        [SerializeField] private float spawnDelay = 1f;

        private int currentWave = 0;
        private int enemiesRemaining = 0;
        private bool _hasStarted = false; 

        private Coroutine _waveCoroutine;

        private void Start()
        {
        
        }

        /// <summary>
        /// Starts the enemy wave spawning when called.
        /// </summary>
        public void StartWaves()
        {
            if (_hasStarted)
            {
                Debug.LogWarning("WaveManager: Waves have already started!");
                return;
            }

            _hasStarted = true;
            _waveCoroutine = StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitUntil(() => Time.timeScale > 0);

            while (currentWave < totalWaves)
            {
                if (GameManager.Instance.CurrentGameState == GameState.GameOver)
                {
                    Debug.Log("WaveManager: Game Over detected. Stopping waves.");
                    yield break;
                }

                currentWave++;
                int enemiesToSpawn = startEnemiesPerWave + (currentWave - 1);
                enemiesRemaining = enemiesToSpawn;

                int enemyIndex = Mathf.Min(currentWave / wavesUntilNewEnemy, enemyPrefabs.Length - 1);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    if (GameManager.Instance.CurrentGameState == GameState.GameOver)
                    {
                        Debug.Log("WaveManager: Game Over detected mid-wave. Stopping enemy spawn.");
                        yield break;
                    }

                    SpawnEnemy(enemyIndex);
                    yield return new WaitForSeconds(spawnDelay);
                }

                yield return new WaitUntil(() => enemiesRemaining <= 0);
            }

            Debug.Log("WaveManager: All waves completed!");
        }

        private void SpawnEnemy(int enemyIndex)
        {
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
            enemy.tag = "Enemy";

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetWaypoints(waypoints);
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
