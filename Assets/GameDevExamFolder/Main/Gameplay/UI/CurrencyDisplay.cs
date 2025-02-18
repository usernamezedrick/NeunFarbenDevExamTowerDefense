using TMPro;
using UnityEngine;
using NF.Main.Gameplay.Managers;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Update()
    {
        if (GameManager.Instance != null && currencyText != null)
        {
            currencyText.text = "Money: $" + GameManager.Instance.currency;
        }
    }
}
