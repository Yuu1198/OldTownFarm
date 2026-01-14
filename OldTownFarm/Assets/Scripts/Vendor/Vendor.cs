using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Vendor : MonoBehaviour
{
    private PlayerController player;
    DialogueData[] vendorDialogue;

    private void Start()
    {
        vendorDialogue = new DialogueData[2];
        vendorDialogue[0] = new DialogueData("Hello my old friend!!!! :]", string.Empty, string.Empty, null, null);
        vendorDialogue[1] = new DialogueData("Sell or buy????", "Yes", "No", OpenSellMenu, OpenBuyMenu);
    }

    void OpenBuyMenu()
    {
        Debug.Log("Hello this is the buy menu");
    }
    void OpenSellMenu()
    {
        Debug.Log("YOU WANT TO SELL???? I dont have money");
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
