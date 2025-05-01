using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Manages the player's actual inventory data: adding, removing, using items.
/// </summary>
public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> itemStacks = new Dictionary<Item, int>();
    public int maxSlots = 30;

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged InventoryChanged;

    public void AddItem(Item item)
    {
        if(itemStacks.ContainsKey(item))
        {
            if(itemStacks[item] < item.maxStack)
            {
                itemStacks[item]++;
                Debug.Log($"Increased {item.itemName} to x{itemStacks[item]}");
            }
            else
            {
                Debug.LogWarning($"{item.itemName} is already at max stack size!");
                return;
            }
        }
        else
        {
            if(itemStacks.Count < maxSlots){
                itemStacks.Add(item, 1);
                Debug.Log($"{item.itemName} added to inventory.");
            }else{
                Debug.LogWarning("Inventory Full!");
                return;
            }
        }
    }

    public bool RemoveItem(Item item)
    {
        if (itemStacks.ContainsKey(item)){
            itemStacks[item]--;
            if (itemStacks[item] <= 0)
                itemStacks.Remove(item);
            InventoryChanged?.Invoke();
            return true;
        }
        else
            return false;
    }

    public void UseItem(Item item)
    {
        if (itemStacks.ContainsKey(item)){
            item.Use();
            RemoveItem(item);
        }
        else
            Debug.LogWarning($"{item.itemName} is not in the inventory!");
    }
    public int GetItemCount(Item item){
    return itemStacks.ContainsKey(item) ? itemStacks[item] : 0;
}
public List<Item> GetAllItems(){
    return itemStacks.Keys.ToList();
}

    public void ToggleInventoryUI()
    {
        // This will be linked to the UI later
    }
}

