using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied = false; // Prevents multiple turrets on the same tile

    public bool CanPlaceTurret()
    {
        return !isOccupied; // Allows placement only if the tile is free
    }
}
