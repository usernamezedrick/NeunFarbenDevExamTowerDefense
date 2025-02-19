using UnityEngine;
using NF.Main.Core;

namespace NF.Main.Gameplay.Managers
{
    public class UIManager : SingletonPersistent<UIManager>
    {
        [SerializeField] private GameObject victoryCanvas;

        private void Start()
        {
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(false); 
            }
        }

        public void ShowVictoryScreen()
        {
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
                Debug.Log("UIManager: Victory Screen Displayed.");
            }
        }

        public void HideVictoryScreen()
        {
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(false);
            }
        }
    }
}
