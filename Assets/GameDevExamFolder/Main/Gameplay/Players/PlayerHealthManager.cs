using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private SpriteRenderer[] hearts; // Drag your 5 heart SpriteRenderers here in the Inspector
    private int currentHealth;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreen; // GameOver UI panel (disabled by default)

    private void Start()
    {
        currentHealth = hearts.Length; // Set health to the number of hearts
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false); // Hide the Game Over screen at start
    }

    /// <summary>
    /// Call this method when the player takes damage.
    /// It disables one of the heart SpriteRenderers.
    /// </summary>
    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            hearts[currentHealth].enabled = false; // Disable the heart at this index
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
            gameOverScreen.SetActive(true); // Show the Game Over screen
        // Optionally, call GameManager.GameOver() here if needed
    }
}
