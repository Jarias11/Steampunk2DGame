using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles displaying the inventory visually (slots, icons, counts).
/// </summary>
public class InventoryController : MonoBehaviour {
    public Inventory inventory;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 30;

    private Slot[] slots;

    private void Start() {
        inventory = GameManager.Instance.playerInventory;
        inventory.InventoryChanged += UpdateInventoryUI;

        slots = new Slot[slotCount];

        for (int i = 0; i < slotCount; i++) {
            GameObject slotGO = Instantiate(slotPrefab, inventoryPanel.transform);
            Slot slot = slotGO.GetComponent<Slot>();
            slots[i] = slot;
            slot.slotIndex = i;
        }

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI() {

        for (int i = 0; i < slots.Length; i++) {
            Inventory.InventoryEntry entry = inventory.GetSlot(i);

            if (entry != null && entry.item != null && entry.count > 0) {
                slots[i].SetItem(entry.item, entry.count);
            }
            else {
                slots[i].ClearSlot();
            }
        }
    }
}


/* what if i want to use the item like apple i want to use which means the player will eat it and restore 20 health, i can see that we would prolly put the logic for that  use with the item but how would i be able to call that use while im in the inventory in the future? */