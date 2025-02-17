using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Core.GameStateMachine; // For GameState enum
using NF.Main.Gameplay.Managers;      // For GameManager
using NF.Main.Gameplay.Towers;        // Added so TowerPlacement is recognized

public class LevelStartController : MonoBehaviour
{
    [SerializeField] private Button startButton;              // UI Start button
    [SerializeField] private TextMeshProUGUI startButtonText;   // TMP text for button label

    [SerializeField] private GameObject waveManagerObject;    // Manually assign in Inspector
    private WaveManager waveManager;
    private GameManager gameManager;
    private bool gameStarted = false;

    private void Start()
    {
        Debug.Log("LevelStartController: Game started in paused state.");

        // Get the GameManager singleton instance.
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("LevelStartController: GameManager is not found in the scene!");
            return;
        }

        // Check if WaveManager was manually assigned in the Inspector.
        if (waveManagerObject == null)
        {
            Debug.LogWarning("LevelStartController: WaveManager object was not assigned in the Inspector. Trying to find it...");
            waveManagerObject = GameObject.Find("WaveManager");

            if (waveManagerObject == null)
            {
                Debug.LogError("LevelStartController: WaveManager object STILL NULL! Check if it's in the scene.");
            }
            else
            {
                Debug.Log("LevelStartController: WaveManager found dynamically.");
            }
        }

        // Get the WaveManager script component.
        if (waveManagerObject != null)
        {
            waveManager = waveManagerObject.GetComponent<WaveManager>();
            waveManagerObject.SetActive(false); // Disable at start
            Debug.Log("LevelStartController: WaveManager script found and disabled at startup.");
        }
        else
        {
            Debug.LogError("LevelStartController: WaveManager object is NULL after search!");
        }

        // Ensure the game is paused at the start.
        Time.timeScale = 0f;
        gameManager.CurrentGameState = GameState.Paused;

        // Set up the button click event.
        startButton.onClick.AddListener(OnStartButtonPressed);
        UpdateButtonText("Start Level");

        Debug.Log("LevelStartController: Waiting for Start Level button press.");
    }

    private void OnStartButtonPressed()
    {
        Debug.Log("LevelStartController: Start button pressed.");

        if (!gameStarted)
        {
            gameStarted = true;
            Time.timeScale = 1f;
            gameManager.CurrentGameState = GameState.Playing;

            if (waveManagerObject != null)
            {
                waveManagerObject.SetActive(true);
                if (waveManager != null)
                {
                    waveManager.StartWaves();
                }
            }

            // Enable turret placement when the game starts
            TowerPlacement[] towers = FindObjectsOfType<TowerPlacement>();
            foreach (TowerPlacement tower in towers)
            {
                tower.EnableTurretPlacement();
            }

            UpdateButtonText("Quit");
        }
        else
        {
            Debug.Log("LevelStartController: Quit button pressed.");
            Application.Quit();
        }
    }

    private void UpdateButtonText(string newText)
    {
        if (startButtonText != null)
        {
            startButtonText.text = newText;
            startButtonText.ForceMeshUpdate();
            Debug.Log("LevelStartController: Button text updated to: " + newText);
        }
    }
}
