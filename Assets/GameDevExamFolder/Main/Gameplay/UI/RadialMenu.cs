using UnityEngine;
using UnityEngine.UI;
using NF.Main.Gameplay.Towers; // For TowerPlacement

namespace NF.Main.UI
{
    /// <summary>
    /// Handles the radial menu UI for turret selection.
    /// </summary>
    public class RadialMenu : MonoBehaviour
    {
        [Header("Turret Options")]
        [SerializeField] private Button turretOption1;
        [SerializeField] private Button turretOption2;
        [SerializeField] private Button turretOption3;
        [SerializeField] private Button exitButton; // Button to close the menu

        [Header("Turret Prefabs")]
        public GameObject turretPrefab1; // Assign turret prefab in Inspector
        public GameObject turretPrefab2;
        public GameObject turretPrefab3;

        private TowerPlacement turretPlacement;

        private void Start()
        {
            // Ensure all buttons are interactable.
            if (turretOption1 != null) turretOption1.interactable = true;
            if (turretOption2 != null) turretOption2.interactable = true;
            if (turretOption3 != null) turretOption3.interactable = true;
            if (exitButton != null) exitButton.interactable = true;

            // Add listener to the exit button.
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(CloseRadialMenu);
            }
        }

        /// <summary>
        /// Set up the radial menu with turret placement context and assign button listeners.
        /// </summary>
        /// <param name="placement">The turret placement controller.</param>
        public void Setup(TowerPlacement placement)
        {
            turretPlacement = placement;

            turretOption1.onClick.AddListener(() => turretPlacement.PlaceTurret(turretPrefab1));
            turretOption2.onClick.AddListener(() => turretPlacement.PlaceTurret(turretPrefab2));
            turretOption3.onClick.AddListener(() => turretPlacement.PlaceTurret(turretPrefab3));
        }

        /// <summary>
        /// Closes the radial menu.
        /// </summary>
        private void CloseRadialMenu()
        {
            turretPlacement?.CloseRadialMenu();
        }
    }
}
