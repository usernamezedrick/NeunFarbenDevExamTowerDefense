using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
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
        [SerializeField] private int totalWaves = 20;
        [SerializeField] private int startEnemiesPerWave = 3;
        [SerializeField] private List<int> enemyThresholds = new List<int> { 3, 6, 9, 12, 15 };
        [SerializeField] private float spawnDelay = 1f;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI waveText; 

        private int currentWave = 0;
        private int enemiesRemaining = 0;
        private bool _hasStarted = false;
        private Coroutine _waveCoroutine;

        private void Start()
        {
            UpdateWaveUI(); 
        }

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
                UpdateWaveUI(); // Update UI when new wave starts

                int enemiesToSpawn = startEnemiesPerWave + (currentWave - 1);
                enemiesRemaining = enemiesToSpawn;

                int[] enemyIndices = GetEnemyIndicesForWave(currentWave);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    if (GameManager.Instance.CurrentGameState == GameState.GameOver)
                    {
                        Debug.Log("WaveManager: Game Over detected mid-wave. Stopping enemy spawn.");
                        yield break;
                    }

                    int randomIndex = enemyIndices[Random.Range(0, enemyIndices.Length)];
                    SpawnEnemy(randomIndex);
                    yield return new WaitForSeconds(spawnDelay);
                }

                yield return new WaitUntil(() => enemiesRemaining <= 0);
            }

            Debug.Log("WaveManager: All waves completed!");
            CheckForVictory();
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
                if (currentWave >= totalWaves)
                {
                    CheckForVictory();
                }
            }
        }

        private int[] GetEnemyIndicesForWave(int wave)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < enemyThresholds.Count; i++)
            {
                if (wave >= enemyThresholds[i])
                {
                    indices.Add(i);
                }
            }

            if (indices.Count == 0)
            {
                indices.Add(0);
            }

            return indices.ToArray();
        }

        private void CheckForVictory()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver) return;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                Debug.Log("WaveManager: All enemies defeated! Triggering Victory.");
                GameManager.Instance.Victory();
            }
        }

        private void UpdateWaveUI()
        {
            if (waveText != null)
            {
                waveText.text = $"Wave: {currentWave}";
            }
        }
    }
}
