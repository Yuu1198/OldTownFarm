using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public PlayerInputActions playerControls;
    public PlayerController player;
    public List<Slots_UI> slots = new List<Slots_UI>();

    [SerializeField] private Canvas canvas;

    private InputAction openInventory;
    private InputAction dragOneItem;
    private Slots_UI draggedSlot;
    private Image draggedIcon;
    private bool dragSingle;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();

        canvas = FindFirstObjectByType<Canvas>();
    }

    private void OnEnable() 
    {
        openInventory = playerControls.UI.OpenInventory;
        openInventory.Enable();
        openInventory.performed += OpenInventory;

        dragOneItem = playerControls.UI.DragOneItem;
        dragOneItem.Enable();
        dragOneItem.performed += HandleDrag;
        dragOneItem.canceled += HandleDrag;
    }

    private void OnDisable() 
    {
        openInventory.Disable();

        dragOneItem.Disable();
    }

    private void OpenInventory(InputAction.CallbackContext context) 
    {
        ToggleInventory();
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

    public void ToggleInventory() 
    {
        if (!inventoryPanel.activeSelf) 
        {
            inventoryPanel.SetActive(true);
            Refresh();
        } 
        else 
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void Refresh() 
    {
        if (slots.Count == player.inventory.slots.Count) 
        {
            for (int i = 0;  i < slots.Count; i++) 
            {
                if (player.inventory.slots[i].itemName != "") 
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                } 
                else 
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove() 
    {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(player.inventory.slots[draggedSlot.slotID].itemName);

        if (itemToDrop != null)
        {
            if (dragSingle)
            {
                // player.DropItem(itemToDrop); // PLACEHOLDER
                player.inventory.Remove(draggedSlot.slotID);
            }
            else
            {
                // player.DropItem(itemToDrop, player.inventory.slots[draggedSlot.slotID].count); // PLACEHOLDER
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }

            Refresh();
        }

        draggedSlot = null;
    }

    public void SlotBeginDrag(Slots_UI slot)
    {
        draggedSlot = slot;
        // Put Icon of Item to drag to mouse position for visual feedback
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false; // Icon does not block Slots
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        // Icon stays at Mouse Position
        MoveToMousePosition(draggedIcon.gameObject);

        Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        // Clean up
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;

        //Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slots_UI slot)
    {
        Debug.Log("Dropped: " + draggedSlot.name + " on " + slot.name);
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            // Convert Mouse Position to Screen Position
            Vector2 position;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Mouse.current.position.ReadValue(), null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }
}
