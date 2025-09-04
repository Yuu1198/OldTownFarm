using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Toolbar_UI : MonoBehaviour
{
    public PlayerInputActions playerControls;

    [SerializeField] private List<Slots_UI> toolbarSlots = new List<Slots_UI>();

    private Slots_UI selectedSlot;
    private InputAction selectSlot;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        selectSlot = playerControls.UI.SelectSlot;
        selectSlot.Enable();
        selectSlot.performed += CheckAlphaNumericKeys;
    }

    private void OnDisable()
    {
        selectSlot.Disable();
    }

    // Will be called when Slot is selected or Number Key is pressed.
    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9)
        {
            Slots_UI previouslySelectedSlot = null;

            // Deselect previously selected slot
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
                previouslySelectedSlot = selectedSlot;
            }

            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);

            GameManager.instance.player.inventoryManager.toolbar.SelectSlot(index); // REFACTURE: make code less entangled

            // Deselect slot if selected again
            if (selectedSlot == previouslySelectedSlot) 
            {
                selectedSlot.SetHighlight(false);
                selectedSlot = null;

                GameManager.instance.player.inventoryManager.toolbar.DeselectSlot();
            }
        }
    }

    private void CheckAlphaNumericKeys(InputAction.CallbackContext context)
    {
        int slotIndex = (int)context.ReadValue<float>();

        slotIndex--;
        SelectSlot(slotIndex);
    }
}
