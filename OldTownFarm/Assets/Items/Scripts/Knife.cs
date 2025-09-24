using UnityEngine.Tilemaps;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Knife Item Data", menuName = "Items/Knife", order = 53)]
public class Knife : ItemData
{
    /*
     * Contains logic for harvesting crops.
     */
    public override void Use(Vector3Int targetTile)
    {
        Crop crop = CropManager.instance.GetCropAtTile(targetTile);

        if (crop != null )
        {
            if (crop.IsHarvestable()) // Check if harvestable on tile
            {
                // Add harvest to inventory
                List<HarvestItem> harvestList = crop.data.harvestItems;
                foreach (HarvestItem harvest in harvestList)
                {
                    Item harvestItem = harvest.item;
                    // Adds as much items as crop can yield
                    for (int i = 0; i < harvest.yield; i++)
                    {
                        GameManager.instance.player.inventoryManager.Add("Backpack", harvestItem);
                    }
                }

                // Delete crop on tile
                CropManager.instance.RemoveCrop(crop);
            }
            if (crop.IsWithered())
            {
                // Delete crop on tile
                CropManager.instance.RemoveCrop(crop);
            }
        }
    }
}
