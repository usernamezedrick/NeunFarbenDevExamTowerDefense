using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Core.GameStateMachine; 
using NF.Main.Gameplay.Managers;     
using NF.Main.Gameplay.Towers;       

public class LevelStartController : MonoBehaviour
{
    [SerializeField] private Button startButton;              
    [SerializeField] private TextMeshProUGUI startButtonText;   

    [SerializeField] private GameObject waveManagerObject;   
    private WaveManager waveManager;
    private GameManager gameManager;
    private bool gameStarted = false;

    private void Start()
    {
        Debug.Log("LevelStartController: Game started in paused state.");

        
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("LevelStartController: GameManager is not found in the scene!");
            return;
        }

        
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

        
        if (waveManagerObject != null)
        {
            waveManager = waveManagerObject.GetComponent<WaveManager>();
            waveManagerObject.SetActive(false); 
            Debug.Log("LevelStartController: WaveManager script found and disabled at startup.");
        }
        else
        {
            Debug.LogError("LevelStartController: WaveManager object is NULL after search!");
        }

        
        Time.timeScale = 0f;
        gameManager.CurrentGameState = GameState.Paused;

       
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
