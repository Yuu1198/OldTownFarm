using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;

    public int sellValueCopper;

    /*
     * Implements the logic of the item (e.g. plowing functionality of hoe)
     */
    public virtual void Use(Vector3Int targetTile)
    {
        Debug.Log($"{itemName} was used which has no implemented logic.");
    }
}
