using UnityEngine;

[CreateAssetMenu(fileName = "WateringCan", menuName = "Items/WateringCan", order = 55)]
public class WateringCan : ItemData
{
    /*
     * Contains logic for watering fields.
     */
    public override void Use(Vector3Int targetTile)
    {
        if (TileManager.instance.GetTypeOfTile(targetTile) == TileManager.TileType.Plowed)
        {
            TileManager.instance.SetTile(targetTile, TileManager.instance.wateredTile);
        }
    }
}
