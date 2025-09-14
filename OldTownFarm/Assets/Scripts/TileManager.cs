using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager instance { get; private set; }

    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;

    [SerializeField] private Tilemap highlightMap;
    [SerializeField] private Tile highlightTile;

    [Header("Tile types:")]
    [SerializeField] public Tile plowedTile;
    [SerializeField] public Tile plantedTile;

    private Vector3Int lastHighlightedTile;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

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
        if (inventoryManager.toolbar.selectedSlot == null || inventoryManager.toolbar.selectedSlot.itemData == null)
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

    public void SetTile(Vector3Int position, Tile newTile)
    {
        interactableMap.SetTile(position, newTile);
    }
}
