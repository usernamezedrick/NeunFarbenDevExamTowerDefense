using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Core.GameStateMachine;  // For GameState enum
using NF.Main.Gameplay.Managers;       // For GameManager

public class LevelStartController : MonoBehaviour
{
    [SerializeField] private Button startButton;              // Your UI Start button
    [SerializeField] private TextMeshProUGUI startButtonText;   // The TMP text for the button label

    private GameManager gameManager;
    private bool gameStarted = false;  // Tracks whether the level has been started
    private GameObject waveManagerObject;

    private void Start()
    {
        // Get the GameManager singleton instance.
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found in the scene!");
            return;
        }

        // Find and disable the WaveManager to prevent enemy spawning.
        waveManagerObject = GameObject.Find("WaveManager");
        if (waveManagerObject != null)
        {
            waveManagerObject.SetActive(false);
            Debug.Log("WaveManager disabled at startup.");
        }
        else
        {
            Debug.LogWarning("WaveManager object not found in the scene.");
        }

        // Ensure the game is paused at the start.
        Time.timeScale = 0f;
        gameManager.CurrentGameState = GameState.Paused;

        // Set up the button click event.
        startButton.onClick.AddListener(OnStartButtonPressed);
        UpdateButtonText("Start Level");

        Debug.Log("Game is paused. Waiting for Start Level button press.");
    }

    private void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed.");
        if (!gameStarted)
        {
            gameStarted = true;
            // Unpause the game.
            Time.timeScale = 1f;
            gameManager.CurrentGameState = GameState.Playing;
            // Enable the WaveManager so it begins spawning enemies.
            if (waveManagerObject != null)
            {
                waveManagerObject.SetActive(true);
                Debug.Log("WaveManager enabled.");
            }
            UpdateButtonText("Quit");
            Debug.Log("Game started: Time.timeScale = " + Time.timeScale);
        }
        else
        {
            Debug.Log("Quit button pressed. Quitting game.");
            Application.Quit();
        }
    }

    private void UpdateButtonText(string newText)
    {
        if (startButtonText != null)
        {
            startButtonText.text = newText;
            startButtonText.ForceMeshUpdate();
            Debug.Log("Button text updated to: " + newText);
        }
    }
}
