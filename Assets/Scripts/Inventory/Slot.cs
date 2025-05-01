using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you are using TextMeshPro for UI text

/// <summary>
/// Represents a single slot in the inventory UI. Handles showing an icon for the item.
/// </summary>
public class Slot : MonoBehaviour {
    public Image icon;
    private Item currentItem;
    public TextMeshProUGUI quantityText; // Assuming you have a TextMeshProUGUI component for item name
    private Vector3 originalScale;
    public void SetItem(Item item) {
        currentItem = item;
        if (icon != null && item.icon != null) {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
            Debug.LogWarning("Icon not assigned: either icon or item.icon is null.");
        if (quantityText != null) {
            int count = GameManager.Instance.playerInventory.GetItemCount(item);
            quantityText.text = $"x{count}";
            quantityText.enabled = true;
        }
    }

    public void ClearSlot() {
        currentItem = null;
        if (icon != null) {
            icon.sprite = null;
            icon.enabled = false;
        }
        if (quantityText != null) {
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }


}