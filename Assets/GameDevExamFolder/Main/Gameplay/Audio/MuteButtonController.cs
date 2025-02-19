using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NF.Main.Gameplay.Audio;

public class MuteButtonController : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private TextMeshProUGUI muteButtonText;

    private void Start()
    {
        if (muteButton == null)
        {
            Debug.LogError("Mute button is not assigned!");
            return;
        }

        muteButton.onClick.AddListener(ToggleMute);
        UpdateButtonText();
    }

    private void ToggleMute()
    {
        SoundManager.Instance.ToggleMute();
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        muteButtonText.text = SoundManager.Instance.IsMuted() ? "Unmute" : "Mute";
        muteButtonText.ForceMeshUpdate();
    }
}
