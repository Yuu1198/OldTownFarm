using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoe Item Data", menuName = "Items/Hoe", order = 51)]
public class Hoe : ItemData
{
    /*
     * Contains logic for plowing fields.
     */
    public override void Use(Vector3Int targetTile)
    {
        TileManager.instance.SetTile(targetTile, TileManager.instance.plowedTile);
    }
}