using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Core.GameStateMachine; // For GameState enum
using NF.Main.Gameplay.Managers;      // For GameManager

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button pauseButton;               // Your UI button
    [SerializeField] private TextMeshProUGUI pauseButtonText;    // The TMP text component on the button

    private bool isPaused = false;
    private GameManager gameManager;

    private void Start()
    {
        // Retrieve the GameManager instance.
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("PauseController: GameManager instance not found!");
        }

        if (pauseButton == null)
            Debug.LogError("PauseButton is not assigned!");
        if (pauseButtonText == null)
            Debug.LogError("PauseButtonText is not assigned!");

        pauseButton.onClick.AddListener(TogglePause);
        UpdateButtonText(); // Initial text update
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        UpdateButtonText();

        if (gameManager == null)
        {
            Debug.LogError("PauseController: GameManager is null in TogglePause!");
            return;
        }

        if (isPaused)
        {
            gameManager.PauseGame();
        }
        else
        {
            gameManager.ResumeGame();
        }
        Debug.Log("PauseController: TogglePause called. isPaused: " + isPaused + ", Time.timeScale: " + Time.timeScale);
    }

    private void UpdateButtonText()
    {
        if (pauseButtonText != null)
        {
            pauseButtonText.text = isPaused ? "Unpause" : "Pause";
            pauseButtonText.ForceMeshUpdate();
            Debug.Log("PauseController: Updated button text: " + pauseButtonText.text);
        }
    }
}
