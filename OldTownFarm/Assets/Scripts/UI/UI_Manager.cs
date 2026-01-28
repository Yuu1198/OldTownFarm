using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public PlayerInputActions playerControls;

    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public GameObject inventoryPanel;
    public GameObject vendorPanel;

    public List<Inventory_UI> inventoryUIs;

    public static Slots_UI draggedSlot;
    public static Image draggedIcon;

    private InputAction openInventory;

    private InputAction dragOneItem;
    public static bool dragSingle;

    [SerializeField] private TextMeshProUGUI moneyText;

    public static UI_Manager instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        playerControls = new PlayerInputActions();

        Initialize();
    }

    private void OnEnable()
    {
        // Inventory
        openInventory = playerControls.UI.OpenInventory;
        openInventory.Enable();
        openInventory.performed += OpenInventory;

        dragOneItem = playerControls.UI.DragOneItem;
        dragOneItem.Enable();
        dragOneItem.performed += HandleDrag;
        dragOneItem.canceled += HandleDrag;

        // Money
        if (PlayerMoneyManager.instance != null)
        {
            PlayerMoneyManager.instance.OnMoneyChanged += UpdateMoneyText;
            
        }
    }

    private void OnDisable()
    {
        openInventory.Disable();

        dragOneItem.Disable();
    }

    // Open inventory through keyboard input
    public void OpenInventory(InputAction.CallbackContext context)
    {
        OpenPlayerInventory(!inventoryPanel.activeSelf);
    }

    // Open or close player inventory through not keyboard input
    public void OpenPlayerInventory(bool open)
    {
        if (inventoryPanel != null)
        {
            DayNightCycle.instance.timeStopped = open;
            inventoryPanel.SetActive(open);
            RefreshInventoryUI("Backpack");
        }
    }

    // Open or close vendor inventory through not keyboard input
    public void OpenVendorInventory(bool open)
    {
        if (vendorPanel != null)
        {
            DayNightCycle.instance.timeStopped = open;
            vendorPanel.SetActive(open);
            RefreshInventoryUI("Vendor");
        }
    }

    // Open inventory through vendor
    public void VendorInteract(bool sell)
    {
        if (sell) // Open Player inventory for selling stuff
        {
            OpenPlayerInventory(true);
            inventoryUIByName["Backpack"].SetSellInv(sell);
        }
        else // Open Vendor inventory for buying stuff
        {
            OpenVendorInventory(true);
        }

    }


    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach (KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    private void HandleDrag(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            dragSingle = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            dragSingle = false;
        }

    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("There is no inventory ui for " + inventoryName);
        return null;
    }

    private void Initialize()
    {
       foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = PlayerMoneyManager.instance.GetMoneyString();
    }
}
