using UnityEngine;
using TMPro;

namespace NF.Main.Gameplay.Managers
{
    public class GoldManager : MonoBehaviour
    {
        [SerializeField] private int startingGold = 100;
        [SerializeField] private TextMeshProUGUI goldText;

        private int _currentGold;

        public static GoldManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _currentGold = startingGold;
            UpdateGoldUI();
        }

        public bool CanAfford(int amount) => _currentGold >= amount;

        public void SpendGold(int amount)
        {
            if (CanAfford(amount))
            {
                _currentGold -= amount;
                UpdateGoldUI();
            }
        }

        public void EarnGold(int amount)
        {
            _currentGold += amount;
            UpdateGoldUI();
        }

        private void UpdateGoldUI()
        {
            if (goldText != null)
                goldText.text = $"Gold: {_currentGold}";
        }
    }
}
