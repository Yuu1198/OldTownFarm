using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake() 
    {
        // Create Inventory with Slots
        inventory = new Inventory(21);
    }

    // Drop Item in world (PLACEHOLDER)
    public void DropItem(Collectable item)
    {
        Vector3 spawnLocation = transform.position;

        // Offset
        Vector3 spawnOffset = Random.insideUnitCircle * 1.5f;

        // Spawn Item
        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        // Make it mory fancy
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }
}
