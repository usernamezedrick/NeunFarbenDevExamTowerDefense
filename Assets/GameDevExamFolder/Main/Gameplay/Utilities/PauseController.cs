using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Gameplay.Managers;
using NF.Main.Core.GameStateMachine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    private GameManager _gameManager;
    private bool _isPaused = false;
    private GameState _lastGameState;
    private bool _gameStarted = false;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        if (pauseButton == null)
            Debug.LogError("PauseButton is not assigned!");
        if (pauseButtonText == null)
            Debug.LogError("PauseButtonText is not assigned!");

        pauseButton.onClick.AddListener(OnButtonPressed);
        pauseButton.interactable = false; // Start disabled
        UpdateButtonText();
    }

    private void Update()
    {
        if (_gameManager == null)
            return;

        if (_lastGameState != _gameManager.CurrentGameState)
        {
            _lastGameState = _gameManager.CurrentGameState;
            UpdateButtonText();
        }

        if (!_gameStarted && _gameManager.HasGameStarted())
        {
            _gameStarted = true;
            pauseButton.interactable = true;
        }
    }

    private void OnButtonPressed()
    {
        if (_gameManager.CurrentGameState == GameState.GameOver || _gameManager.CurrentGameState == GameState.Victory)
        {
            RestartGame();
        }
        else
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (!_gameStarted) return;

        _isPaused = !_isPaused;
        if (_isPaused)
            _gameManager.PauseGame();
        else
            _gameManager.ResumeGame();

        UpdateButtonText();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateButtonText()
    {
        if (pauseButtonText == null) return;

        if (_gameManager.CurrentGameState == GameState.GameOver || _gameManager.CurrentGameState == GameState.Victory)
        {
            pauseButtonText.text = "Play Again";
        }
        else
        {
            pauseButtonText.text = _isPaused ? "Unpause" : "Pause";
        }

        pauseButtonText.ForceMeshUpdate();
    }
}
