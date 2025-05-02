using UnityEngine;
using UnityEngine.UI;

public class DragManager : MonoBehaviour {
    public static DragManager Instance;

    public Image dragImage;
    public Item draggedItem;
    public int sourceSlotIndex = -1;
    public Slot sourceSlot;
    private void Awake() {
        Instance = this;
        HideDrag();
    }

    private void Update() {
        if (dragImage.gameObject.activeSelf) {
            dragImage.transform.position = Input.mousePosition;
        }
    }

    public void ShowDrag(Item item, Sprite sprite, Slot slot) {
        draggedItem = item;
        sourceSlot = slot;
        sourceSlotIndex = slot.slotIndex; // optional if you still want the index
        dragImage.sprite = sprite;
        dragImage.color = Color.white;
        dragImage.gameObject.SetActive(true);
    }

    public void HideDrag() {
        draggedItem = null;
        sourceSlot = null;
        sourceSlotIndex = -1;
        dragImage.color = new Color(1, 1, 1, 0);
        dragImage.gameObject.SetActive(false);
    }
}
