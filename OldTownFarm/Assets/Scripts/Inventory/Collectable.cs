using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;
    public Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();

        // Player collects this Collectable and it gets added to Inventory
        if (player) {
            player.inventory.Add(this);
            Destroy(this.gameObject);
        }
    }
}

// All possible Types of an Collectable
public enum CollectableType {
    none,
    carrotSeed
}
