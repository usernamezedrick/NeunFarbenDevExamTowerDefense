using UnityEngine;
using NF.Main.Gameplay.Managers;

namespace NF.Main.Gameplay
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private SpriteRenderer[] hearts;
        private int currentHealth;

        [Header("Game Over Screen")]
        [SerializeField] private GameObject gameOverScreen;

        private void Start()
        {
            currentHealth = hearts.Length; // Set health to the number of hearts

            if (gameOverScreen != null)
                gameOverScreen.SetActive(false);
        }

        public void TakeDamage()
        {
            if (currentHealth > 0)
            {
                currentHealth--;
                hearts[currentHealth].enabled = false;
                Debug.Log("PlayerHealthManager: Heart lost. Remaining hearts: " + currentHealth);
            }

            if (currentHealth <= 0)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            Debug.Log("PlayerHealthManager: All hearts lost. Game Over!");
            if (gameOverScreen != null)
                gameOverScreen.SetActive(true);

            GameManager.Instance.GameOver(); 
        }
    }
}
