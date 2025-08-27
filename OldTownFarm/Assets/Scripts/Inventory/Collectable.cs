using UnityEngine;

[RequireComponent (typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // Player collects this Collectable and it gets added to Inventory
        if (player) 
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {
                player.inventory.Add(item);
                Destroy(this.gameObject);
            }
        }
    }
}
