using System.Collections.Generic;
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
    [SerializeField] public List<Tile> grassTiles;
    [SerializeField] public Tile plowedTile;
    [SerializeField] public Tile wateredTile;

    private Vector3Int lastHighlightedTile;

    private int daysTillFieldReset = 3;

    public enum TileType
    {
        None,
        Grass,
        Plowed,
        Watered
    }

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

        DayNightCycle.instance.OnDayChanged.AddListener(OnDayChangedHandler);
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

    public TileType GetTypeOfTile(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                if (tile.name == plowedTile.name)
                {
                    return TileType.Plowed;
                }
                else if (tile.name == wateredTile.name)
                {
                    return TileType.Watered;
                }
                else
                {
                    return TileType.Grass;
                }
            }

            return TileType.None;

        }

        return TileType.None;
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

    /// <summary>
    /// Resets Tile back to Grass Tile when no Crop on Tile
    /// </summary>
    /// <param name="position"> Position of Tile</param>
    private void ResetTile(Vector3Int position)
    {
        if (CropManager.instance.GetCropAtTile(position) == null)
        {
            Tile grassTileToSet = grassTiles[Random.Range(0, grassTiles.Count)];
            SetTile(position, grassTileToSet);
        }
    }

    /// <summary>
    /// Set all Plowed Tiles to random Grass Tiles.
    /// </summary>
    private void ResetFields()
    {
        if (interactableMap != null)
        {
            BoundsInt bounds = interactableMap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = interactableMap.GetTile(pos);

                if (tile != null && tile.name == plowedTile.name)
                {
                    ResetTile(pos);
                }
            }
        }
    }

    /// <summary>
    /// Set all Watered Tiles to Plowed Tiles.
    /// </summary>
    private void DryTiles()
    {
        if (interactableMap != null)
        {
            BoundsInt bounds = interactableMap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = interactableMap.GetTile(pos);

                if (tile != null && tile.name == wateredTile.name)
                {
                    interactableMap.SetTile(pos, plowedTile);
                }
            }
        }
    }

    private void OnDayChangedHandler(int day)
    {
        ResetFields();

        DryTiles();
    }
}
