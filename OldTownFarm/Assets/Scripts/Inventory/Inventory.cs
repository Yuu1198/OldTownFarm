using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    [System.Serializable]
    public class Slot 
    {
        public string itemName;
        public int count;
        public int maxAllowed;
        public Sprite icon;

        public Slot() 
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool CanAddItem() 
        {
            if (count < maxAllowed) 
            {
                return true;
            } 
            else 
            {
                return false;
            }
        }

        public void AddItem(Item item) {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
            count++;
        }

        public void RemoveItem() 
        {
            // Remove Item from Slot if at least one Item is in it
            if (count > 0)
            { 
                count--;

                // Slot is empty
                if (count == 0) 
                {
                    icon = null;
                    itemName = "";
                }
            }
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Inventory(int numSlots) 
    {
        // Add slots to inventory
        for (int i = 0; i < numSlots; i++) 
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    public void Add(Item item) 
    {
        // Find Slot with same type of Collectable
        foreach (Slot slot in slots) 
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem()) 
            {
                slot.AddItem(item);
                return;
            }
        }
        // No same type of Slot found
        // Add Collectable to empty slot
        foreach (Slot slot in slots) 
        {
            if (slot.itemName == "") 
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index) 
    {
        slots[index].RemoveItem();
    }
}
