using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSlots = 30;
    public int selectedSlotIndex = 0;
    public bool isInventoryOpen = false;
    public void AddItem(Item item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
            Debug.Log(item.itemName + " added.");
        }
        else
        {
            Debug.Log("Inventory Full!");
        }
    }
    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }
    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (Item i in items)
        {
            if (i == item) count++;
        }
        return count;
    }
    public void UseSelectedItem()
    {
        if (selectedSlotIndex < items.Count)
        {
            Item itemToUse = items[selectedSlotIndex];
            Debug.Log("Using: " + itemToUse.itemName);
            // Add custom behavior based on item type here
        }
    }
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        // Add code to show/hide UI
    }
    public void SelectSlot(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            selectedSlotIndex = index;
        }
    }
}

