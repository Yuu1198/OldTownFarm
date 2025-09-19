using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed Item Data", menuName = "Items/Seeds", order = 52)]
public class Seeds : ItemData
{
    [SerializeField] private CropData crop;

    /*
     * Contains logic for seeding crops.
     */
    public override void Use(Vector3Int targetTile)
    {
        // Only allow seeding on plowed tiles.
        if ((TileManager.instance.GetTypeOfTile(targetTile) == TileManager.TileType.Plowed || TileManager.instance.GetTypeOfTile(targetTile) == TileManager.TileType.Watered) && CropManager.instance.GetCropAtTile(targetTile) == null)
        {
            GameManager.instance.player.inventoryManager.toolbar.selectedSlot.RemoveItem(); // this is a bit dirty, maybe REFACTURE?
            GameManager.instance.uiManager.RefreshAll();
            //Instantiate(crop, targetTile + new Vector3(0.5f, 0.3f), Quaternion.identity);
            CropManager.instance.SeedCrop(targetTile, crop);
        }
    }
}