using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoe Item Data", menuName = "Items/Hoe", order = 51)]
public class HoeItemData : ItemData
{
    public override void Use(Vector3Int targetTile)
    {
        TileManager.Instance.SetTile(targetTile, TileManager.Instance.plowedTile);
        Debug.Log("Hoeing");
    }
}