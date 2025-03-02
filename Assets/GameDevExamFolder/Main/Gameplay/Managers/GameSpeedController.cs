using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSpeedController : MonoBehaviour
{
    [SerializeField] private Button speedButton; // Assign your UI Button
    private TMP_Text speedLabel; // Will be auto-found inside the button
    private bool isFastMode = false;

    private void Start()
    {
        // Get the TMP Text from the button's child
        speedLabel = speedButton.GetComponentInChildren<TMP_Text>();

        // Make sure the button works
        speedButton.onClick.AddListener(ToggleSpeed);
        UpdateSpeedLabel();
    }

    private void ToggleSpeed()
    {
        isFastMode = !isFastMode;
        Time.timeScale = isFastMode ? 2f : 1f;
        UpdateSpeedLabel();
    }

    private void UpdateSpeedLabel()
    {
        if (speedLabel != null)
        {
            speedLabel.text = isFastMode ? "2x" : "1x";
        }
    }
}
