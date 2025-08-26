using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public PlayerInputActions playerControls;
    public Player player;
    public List<Slots_UI> slots = new List<Slots_UI>();

    private InputAction openInventory;

    private void Awake() {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable() {
        openInventory = playerControls.UI.OpenInventory;
        openInventory.Enable();
        openInventory.performed += OpenInventory;
    }

    private void OnDisable() {
        openInventory.Disable();
    }

    private void OpenInventory(InputAction.CallbackContext context) {
        ToggleInventory();
    }

    public void ToggleInventory() {
        if (!inventoryPanel.activeSelf) {
            inventoryPanel.SetActive(true);
            Setup();
        } else {
            inventoryPanel.SetActive(false);
        }
    }

    public void Setup() {
        if (slots.Count == player.inventory.slots.Count) {
            for (int i = 0;  i < slots.Count; i++) {
                if (player.inventory.slots[i].type != CollectableType.none) {
                    slots[i].SetItem(player.inventory.slots[i]);
                } else {
                    slots[i].SetEmpty();
                }
            }
        }
    }
}
