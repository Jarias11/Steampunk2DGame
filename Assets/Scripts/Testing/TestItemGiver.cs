using UnityEngine;

public class TestItemGiver : MonoBehaviour {
    [Header("Item to Give")]
    public Item itemToGive;

    [Header("Quantity")]
    [Min(1)]
    public int amount = 1;

    private void Start() {
        if (itemToGive != null && amount > 0) {
            for (int i = 0; i < amount; i++) {
                GameManager.Instance.playerInventory.AddItem(itemToGive);

            }
            GameManager.Instance.playerInventory.ForceUpdate();

            // Force a UI update immediately after giving items


            Debug.Log($"Added {amount}x {itemToGive.itemName} to inventory!");
        }
    }
}