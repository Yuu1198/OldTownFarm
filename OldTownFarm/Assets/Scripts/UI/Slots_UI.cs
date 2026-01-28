using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots_UI : MonoBehaviour, IPointerClickHandler
{
    public int slotID;
    private Inventory.Slot slotData;
    public Inventory inventory;

    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    // Set up filled slot
    public void SetItem(Inventory.Slot slot) {
        if (slot != null) {
            slotData = slot;
            itemIcon.sprite = slot.itemData.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    // Set up empty slot
    public void SetEmpty() {
        slotData = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotData != null)
        {
            if (inventory.sellInventory)
            {
                // Sell item on right click
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    int sellValue = slotData.itemData.sellValueCopper;

                    PlayerMoneyManager.instance.AddMoney(new Currency(0, 0, sellValue));

                    slotData.RemoveItem();
                    UI_Manager.instance.inventoryUIByName["Backpack"].Refresh();
                }
            }
            if (inventory.buyInventory)
            {
                // Buy item on right click
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    int buyValue = slotData.itemData.sellValueCopper;

                    if (buyValue > PlayerMoneyManager.instance.GetTotalCopper())
                    {
                        Debug.Log("Nicht genug Geld");
                        return;
                    }

                    PlayerMoneyManager.instance.SubstractMoney(new Currency(0, 0, buyValue));

                    GameManager.instance.player.inventoryManager.GetInventoryByName("Backpack").AddItem(slotData.itemData, 99);

                    slotData.RemoveItem();
                    UI_Manager.instance.inventoryUIByName["Vendor"].Refresh();
                }
            }
        }
    }
}
