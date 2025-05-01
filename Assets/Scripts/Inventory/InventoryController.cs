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

        // Create slots
        for (int i = 0; i < slotCount; i++) {
            GameObject slotGO = Instantiate(slotPrefab, inventoryPanel.transform);
            Slot slot = slotGO.GetComponent<Slot>();
            slots[i] = slot;
        }

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI() {
        List<Item> items = inventory.GetAllItems();
        for (int i = 0; i < slots.Length; i++) {
            if (i < items.Count) {
                slots[i].SetItem(items[i]);
            }
            else {
                slots[i].ClearSlot();
            }
        }
    }
}
