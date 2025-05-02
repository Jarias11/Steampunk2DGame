using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Manages the player's actual inventory data: adding, removing, using items.
/// </summary>
public class Inventory : MonoBehaviour {


    [System.Serializable]
    public class InventoryEntry {
        public Item item;
        public int count;

        public InventoryEntry(Item item, int count) {
            this.item = item;
            this.count = count;
        }
    }










    public List<InventoryEntry> slots = new List<InventoryEntry>();
    public int maxSlots = 30;

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged InventoryChanged;




    private void Awake() {
        // Initialize inventory with empty slots
        for (int i = 0; i < maxSlots; i++) {
            slots.Add(new InventoryEntry(null, 0));
        }
    }



    public void AddItem(Item item, int quantity = 1) {
        // Try to stack first
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].item == item && slots[i].count < item.maxStack) {
                int spaceLeft = item.maxStack - slots[i].count;
                int amountToAdd = Mathf.Min(quantity, spaceLeft);
                slots[i].count += amountToAdd;
                quantity -= amountToAdd;
                if (quantity <= 0) {
                    InventoryChanged?.Invoke();
                    return;
                }
            }
        }

        // Add to empty slots
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].item == null) {
                int amountToAdd = Mathf.Min(quantity, item.maxStack);
                slots[i].item = item;
                slots[i].count = amountToAdd;
                quantity -= amountToAdd;
                if (quantity <= 0) {
                    InventoryChanged?.Invoke();
                    return;
                }
            }
        }

        Debug.LogWarning("Inventory is full!");
        InventoryChanged?.Invoke();
    }

    public void RemoveItem(int slotIndex, int amount = 1) {
        if (slotIndex >= 0 && slotIndex < slots.Count) {
            InventoryEntry slot = slots[slotIndex];
            if (slot.item != null) {
                slot.count -= amount;
                if (slot.count <= 0) {
                    slot.item = null;
                    slot.count = 0;
                }

                InventoryChanged?.Invoke();
            }
        }
    }

    public void SetSlot(int index, Item item, int count) {
        if (index >= 0 && index < slots.Count) {
            slots[index].item = item;
            slots[index].count = count;
            InventoryChanged?.Invoke();
        }
    }

    public void SwapSlots(int indexA, int indexB) {
        if (indexA == indexB || indexA < 0 || indexB < 0 || indexA >= slots.Count || indexB >= slots.Count) return;

        var temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        InventoryChanged?.Invoke();
    }

    public InventoryEntry GetSlot(int index) {
        return index >= 0 && index < slots.Count ? slots[index] : null;
    }


    public void ForceUpdate() {
        InventoryChanged?.Invoke();
    }
    public List<SavedItemSlot> ToSavedData() {
        List<SavedItemSlot> saved = new List<SavedItemSlot>();
        for (int i = 0; i < slots.Count; i++) {
            var slot = slots[i];
            if (slot.item != null && slot.count > 0) {
                saved.Add(new SavedItemSlot {
                    slotIndex = i,
                    itemID = slot.item.itemID,
                    count = slot.count
                });
            }
        }
        return saved;
    }

    public void LoadFromSavedData(List<SavedItemSlot> savedSlots) {
        // Clear current inventory
        for (int i = 0; i < slots.Count; i++) {
            slots[i].item = null;
            slots[i].count = 0;
        }

        foreach (var saved in savedSlots) {
            Item item = ItemDatabase.GetItemByID(saved.itemID);
            if (item != null && saved.slotIndex >= 0 && saved.slotIndex < slots.Count) {
                slots[saved.slotIndex].item = item;
                slots[saved.slotIndex].count = saved.count;
            }
            else {
                Debug.LogWarning($"❌ Failed to restore item '{saved.itemID}' at slot {saved.slotIndex}");
            }
        }

        InventoryChanged?.Invoke();
    }
}

