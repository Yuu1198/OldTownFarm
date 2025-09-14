using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed Item Data", menuName = "Items/Seeds", order = 52)]
public class Seeds : ItemData
{
    [SerializeField] private GameObject crop;

    /*
     * Contains logic for seeding crops.
     */
    public override void Use(Vector3Int targetTile)
    {
        // Only allow seeding on plowed tiles.
        if (TileManager.Instance.GetTileName(targetTile) == TileManager.Instance.plowedTile.name)
        {
            TileManager.Instance.SetTile(targetTile, TileManager.Instance.plantedTile);
            GameManager.instance.player.inventoryManager.toolbar.selectedSlot.RemoveItem(); // this is a bit dirty, maybe REFACTURE?
            GameManager.instance.uiManager.RefreshAll();
            Instantiate(crop, targetTile + new Vector3(0.5f, 0.3f), Quaternion.identity);
        }
    }
}