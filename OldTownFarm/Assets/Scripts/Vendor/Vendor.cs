using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Vendor : MonoBehaviour
{
    private PlayerController player;
    DialogueData[] vendorDialogue;

    public ItemData[] possibleItemsToBuy;

    [SerializeField]
    private Inventory vendorInventory;

    private void Start()
    {
        // Fill Dialogue
        vendorDialogue = new DialogueData[2];
        vendorDialogue[0] = new DialogueData("Hello my old friend!!!! :]", string.Empty, string.Empty, null, null);
        vendorDialogue[1] = new DialogueData("Sell or buy????", "Yes", "No", OpenSellMenu, OpenBuyMenu);

        
        vendorInventory = GameManager.instance.player.inventoryManager.GetInventoryByName("Vendor");
        AddItemsToVendorInventory();
    }

    // Fill Vendor Inventory MAKE RANDOM IN FUTURE TOOODOOO and make it new every day
    private void AddItemsToVendorInventory()
    {
        vendorInventory.AddItem(possibleItemsToBuy[0], 1);
        vendorInventory.AddItem(possibleItemsToBuy[1], 2, 2);
        vendorInventory.AddItem(possibleItemsToBuy[0], 1);
        vendorInventory.AddItem(possibleItemsToBuy[1], 1, 1);
    }

    void OpenBuyMenu()
    {
        Debug.Log("Hello this is the buy menu");

        UI_Manager.instance.VendorInteract(false);
        Dialogue.instance.KillDialogue();

    }

    void OpenSellMenu()
    {
        Debug.Log("YOU WANT TO SELL???? I dont have money");

        UI_Manager.instance.VendorInteract(true);
        Dialogue.instance.KillDialogue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

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
            // DO SMTH
            Dialogue.instance.StartDialogue(vendorDialogue);
        }
    }
}
