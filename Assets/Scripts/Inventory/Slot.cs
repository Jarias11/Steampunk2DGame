using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a single slot in the inventory UI. Handles showing an icon, count, and drag/drop.
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
    public Image icon;
    public TextMeshProUGUI quantityText;
    public int slotIndex;

    private Item currentItem;
    private int currentCount;

    public void SetItem(Item item, int count) {
        currentItem = item;
        currentCount = count;

        if (icon != null && item.icon != null) {
            icon.sprite = item.icon;
            icon.enabled = true;
        }

        if (quantityText != null) {
            quantityText.text = $"x{count}";
            quantityText.enabled = true;
        }
    }

    public void ClearSlot() {
        currentItem = null;
        currentCount = 0;

        if (icon != null) {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (quantityText != null) {
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (currentItem != null) {
            icon.rectTransform.localScale = Vector3.one * 1.2f;
            TooltipController.Instance.ShowTooltip(currentItem.itemName, transform as RectTransform);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        icon.rectTransform.localScale = Vector3.one;
        TooltipController.Instance.HideTooltip();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (currentItem != null) {
            DragManager.Instance.ShowDrag(currentItem, icon.sprite, this);
        }
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData) {
        DragManager.Instance.HideDrag();
    }

    public void OnDrop(PointerEventData eventData) {
        Slot fromSlot = DragManager.Instance.sourceSlot;
        Slot toSlot = this;

        if (fromSlot == null || fromSlot == toSlot) return;

        Inventory inv = GameManager.Instance.playerInventory;

        var fromEntry = inv.GetSlot(fromSlot.slotIndex);
        var toEntry = inv.GetSlot(toSlot.slotIndex);

        if (fromEntry == null || fromEntry.item == null) return;

        // Swap or move using slot indices
        inv.SwapSlots(fromSlot.slotIndex, toSlot.slotIndex);
        inv.ForceUpdate();
    }

    public Item GetCurrentItem() {
        return currentItem;
    }
}