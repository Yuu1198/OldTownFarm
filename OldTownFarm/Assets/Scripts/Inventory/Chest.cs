using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;
    private SpriteRenderer spriteRenderer;

    private PlayerController player;
    [SerializeField] private Inventory_UI toolbarUI;

    public Currency chestMoney;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Visuals
            spriteRenderer.sprite = open;

            player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SubscribeToInteract(OnInteract);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Visuals
            spriteRenderer.sprite = close;

            if (player != null)
            {
                player.UnsubscribeFromInteract(OnInteract);
                player = null;
            }
        }
    }

    private void OnInteract(CallbackContext context)
    {
        if (player != null)
        {
            var equippedItem = player.inventoryManager.toolbar.selectedSlot;

            if (equippedItem != null && equippedItem.itemData != null)
            {
                int sellValue = equippedItem.itemData.sellValueCopper;

                // Add to chest money
                chestMoney.Add(new Currency(0, 0, sellValue));

                // Remove item from inventory
                equippedItem.RemoveItem();
                toolbarUI.Refresh();
            }
        }
    }
}
