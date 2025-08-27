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
        PlayerController player = collision.GetComponent<PlayerController>();

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
