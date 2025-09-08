using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile plowedTile;

    [SerializeField] private Tilemap highlightMap;
    [SerializeField] private Tile highlightTile;

    private Vector3Int lastHighlightedTile;

    void Start()
    {
        // Hide interactable tiles
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null && tile.name == "interactable_visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }
    }

    // REFACTURE: not only handle hoe tool but others too (pass tool (enum), different tiles)
    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return null;
    }

    public Vector3Int GetTargetTile(Vector3 playerPosition, Vector2Int facingDirection)
    {
        Vector3Int playerTile = interactableMap.WorldToCell(playerPosition);
        Vector3Int targetTile = playerTile + new Vector3Int(facingDirection.x, facingDirection.y, 0);

        return targetTile;
    }

    public void HighlightTile(Vector3 playerPosition, Vector2Int facingDirection, InventoryManager inventoryManager)
    {
        // Clear when no Tool selected
        if (inventoryManager.toolbar.selectedSlot == null || string.IsNullOrWhiteSpace(inventoryManager.toolbar.selectedSlot.itemName))
        {
            if (highlightMap.HasTile(lastHighlightedTile))
            {
                highlightMap.SetTile(lastHighlightedTile, null);
            }

            return;
        }

        Vector3Int targetTile = GetTargetTile(playerPosition, facingDirection);

        // Clear old
        if (highlightMap.HasTile(lastHighlightedTile))
        {
            highlightMap.SetTile(lastHighlightedTile, null);
        }

        // Set new
        string targetTileName = GetTileName(targetTile);
        if (!string.IsNullOrWhiteSpace(targetTileName))
        {
            //if (targetTileName == "interactable")
            {
                
                highlightMap.SetTile(targetTile, highlightTile);
                lastHighlightedTile = targetTile;
            }
        }
    }
}
