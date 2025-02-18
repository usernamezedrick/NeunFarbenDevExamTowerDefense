using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Gameplay.Managers;
using NF.Main.Core.GameStateMachine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    private GameManager _gameManager;
    private bool _isPaused = false;
    private GameState _lastGameState;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        if (pauseButton == null)
            Debug.LogError("PauseButton is not assigned!");
        if (pauseButtonText == null)
            Debug.LogError("PauseButtonText is not assigned!");

        pauseButton.onClick.AddListener(TogglePause);
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

        if (!_gameManager.HasGameStarted())
        {
            pauseButton.interactable = false;
        }
        else
        {
            pauseButton.interactable = true;
        }
    }

    private void TogglePause()
    {
        if (!_gameManager.HasGameStarted()) return;

        if (_gameManager.CurrentGameState == GameState.GameOver ||
            _gameManager.CurrentGameState == GameState.Victory)
        {
            _gameManager.Initialize();
        }
        else
        {
            _isPaused = !_isPaused;
            if (_isPaused)
                _gameManager.PauseGame();
            else
                _gameManager.ResumeGame();
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (pauseButtonText == null) return;

        if (_gameManager.CurrentGameState == GameState.GameOver)
        {
            pauseButtonText.text = "Try Again";
        }
        else if (_gameManager.CurrentGameState == GameState.Victory)
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
