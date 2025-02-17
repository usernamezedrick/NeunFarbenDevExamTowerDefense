using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NF.Main.Core.GameStateMachine;
using NF.Main.Gameplay.Managers;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    private bool isPaused = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (pauseButton == null)
            Debug.LogError("PauseButton is not assigned!");
        if (pauseButtonText == null)
            Debug.LogError("PauseButtonText is not assigned!");

        pauseButton.onClick.AddListener(TogglePause);
        UpdateButtonText();
    }

    private void Update()
    {
        
        if (gameManager.CurrentGameState == GameState.GameOver)
        {
            UpdateButtonText();
        }
    }

    private void TogglePause()
    {
       
        if (gameManager.CurrentGameState == GameState.GameOver)
        {
            RestartGame();
            return;
        }

       
        isPaused = !isPaused;
        UpdateButtonText();

        if (isPaused)
        {
            gameManager.PauseGame();
        }
        else
        {
            gameManager.ResumeGame();
        }

        Debug.Log("TogglePause called. isPaused: " + isPaused);
    }

    private void UpdateButtonText()
    {
        if (pauseButtonText != null)
        {
            if (gameManager.CurrentGameState == GameState.GameOver)
            {
                pauseButtonText.text = "Try Again"; 
            }
            else
            {
                pauseButtonText.text = isPaused ? "Unpause" : "Pause";
            }
            pauseButtonText.ForceMeshUpdate();
            Debug.Log("Updated button text: " + pauseButtonText.text);
        }
    }

    private void RestartGame()
    {
        Debug.Log("Restarting Game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
