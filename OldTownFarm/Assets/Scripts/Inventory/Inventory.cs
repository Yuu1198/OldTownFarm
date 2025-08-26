using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {
    [System.Serializable]
    public class Slot {
        public CollectableType type;
        public int count;
        public int maxAllowed;
        public Sprite icon;

        public Slot() {
            type = CollectableType.none;
            count = 0;
            maxAllowed = 99;
        }

        public bool CanAddItem() {
            if (count < maxAllowed) {
                return true;
            } else {
                return false;
            }
        }

        public void AddItem(Collectable item) {
            this.type = item.type;
            this.icon = item.icon;
            count++;
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Inventory(int numSlots) {
        // Add slots to inventory
        for (int i = 0; i < numSlots; i++) {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    public void Add(Collectable item) {
        // Find Slot with same type of Collectable
        foreach (Slot slot in slots) {
            if (slot.type == item.type && slot.CanAddItem()) {
                slot.AddItem(item);
                return;
            }
        }
        // No same type of Slot found
        // Add Collectable to empty slot
        foreach (Slot slot in slots) {
            if (slot.type == CollectableType.none) {
                slot.AddItem(item);
                return;
            }
        }
    }
}
