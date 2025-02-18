using UnityEngine;
using NF.Main.Gameplay.Managers; // For GameManager
using NF.Main.UI;              // For RadialMenu

namespace NF.Main.Gameplay.Towers
{
    public class TowerPlacement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject radialMenuPrefab;
        [SerializeField] private Transform canvas;

        private Camera mainCamera;
        private GameObject currentRadialMenu;
        private Tile selectedTile;

        private bool canPlaceTurret = false;
        private bool isRadialMenuActive = false;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Prevent turret placement if the game hasn't started or is paused.
            if (!GameManager.Instance.HasGameStarted() || GameManager.Instance.IsGamePaused())
                return;

            if (Input.GetMouseButtonDown(0) && canPlaceTurret && !isRadialMenuActive)
            {
                HandleTileClick();
            }
        }

        private void HandleTileClick()
        {
            if (isRadialMenuActive)
                return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);

            if (hit.collider != null)
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null && tile.CanPlaceTurret())
                {
                    if (currentRadialMenu != null)
                        Destroy(currentRadialMenu);

                    selectedTile = tile;

                    currentRadialMenu = Instantiate(radialMenuPrefab, canvas);
                    currentRadialMenu.transform.position = mainCamera.WorldToScreenPoint(tile.transform.position);

                    // Set up the radial menu.
                    RadialMenu radialMenu = currentRadialMenu.GetComponent<RadialMenu>();
                    if (radialMenu != null)
                    {
                        radialMenu.Setup(this);
                    }
                    else
                    {
                        Debug.LogError("RadialMenu component not found on the radialMenuPrefab!");
                    }

                    isRadialMenuActive = true;
                    canPlaceTurret = false;
                    Invoke(nameof(ResetTowerPlacement), 0.5f);
                }
            }
        }

        private void ResetTowerPlacement()
        {
            canPlaceTurret = true;
        }

        /// <summary>
        /// Allows turret placement (e.g., when the game starts or resumes).
        /// </summary>
        public void EnableTurretPlacement()
        {
            canPlaceTurret = true;
        }

        /// <summary>
        /// Disables turret placement (e.g., when the game is paused).
        /// </summary>
        public void DisableTurretPlacement()
        {
            canPlaceTurret = false;
        }

        /// <summary>
        /// Called by the RadialMenu when a turret option is selected.
        /// </summary>
        /// <param name="turretPrefab">The turret prefab to instantiate.</param>
        /// <param name="turretCost">The cost of the turret.</param>
        public void PlaceTurret(GameObject turretPrefab, int turretCost)
        {
            if (selectedTile != null && turretPrefab != null)
            {
                if (!GameManager.Instance.CanAfford(turretCost))
                {
                    Debug.Log("Not enough currency!");
                    return;
                }
                GameManager.Instance.SpendCurrency(turretCost);

                // Adjust z so the turret appears above the tile.
                Vector3 turretPos = selectedTile.transform.position;
                turretPos.z = -1f;
                Instantiate(turretPrefab, turretPos, Quaternion.identity);
                selectedTile.isOccupied = true;
                if (currentRadialMenu != null)
                    Destroy(currentRadialMenu);
                isRadialMenuActive = false;
            }
            else
            {
                Debug.LogError("Either selectedTile or turretPrefab is null in PlaceTurret().");
            }
        }

        /// <summary>
        /// Called by the RadialMenu to close and cancel turret placement.
        /// </summary>
        public void CloseRadialMenu()
        {
            if (currentRadialMenu != null)
            {
                Destroy(currentRadialMenu);
                isRadialMenuActive = false;
            }
        }
    }
}
