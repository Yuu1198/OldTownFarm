using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();

        // Player collects this Collectable and it gets added to Inventory
        if (player) {
            player.inventory.Add(type);
            Destroy(this.gameObject);
        }
    }
}

// All possible Types of an Collectable
public enum CollectableType {
    none,
    carrotSeed
}
