using UnityEngine;
using NF.Main.UI;  // Import the namespace where RadialMenu is defined

namespace NF.Main.Gameplay.Towers
{
    public class TowerPlacement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject radialMenuPrefab; // Prefab for the turret selection menu.
        [SerializeField] private Transform canvas;            // Canvas for spawning the radial menu.

        private Camera mainCamera;
        private GameObject currentRadialMenu;
        private Tile selectedTile;  // Tile script attached to each grid tile

        private bool canPlaceTurret = true;
        private bool isRadialMenuActive = false;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // On left mouse click, if placement is allowed and the radial menu isn't active, handle tile click.
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

                    // Instantiate the radial menu on the UI canvas at the tile's screen position.
                    currentRadialMenu = Instantiate(radialMenuPrefab, canvas);
                    currentRadialMenu.transform.position = mainCamera.WorldToScreenPoint(tile.transform.position);

                    // Set up the radial menu with this TowerPlacement instance.
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
                    Invoke(nameof(ResetTowerPlacement), 0.5f); // Reset click allowance after a short delay.
                }
            }
        }

        private void ResetTowerPlacement()
        {
            canPlaceTurret = true;
        }

        /// <summary>
        /// Called by the radial menu when a turret option is selected.
        /// </summary>
        /// <param name="turretPrefab">The turret prefab to instantiate.</param>
        public void PlaceTurret(GameObject turretPrefab)
        {
            if (selectedTile != null && turretPrefab != null)
            {
                Instantiate(turretPrefab, selectedTile.transform.position, Quaternion.identity);
                selectedTile.isOccupied = true;  // Mark tile as occupied so no further turret is placed here.
                if (currentRadialMenu != null)
                    Destroy(currentRadialMenu);
                isRadialMenuActive = false;
            }
        }

        /// <summary>
        /// Called by the radial menu to close and cancel turret placement.
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
