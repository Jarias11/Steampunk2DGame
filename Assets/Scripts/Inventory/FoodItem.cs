using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Items/Food")]
public class FoodItem : Item {
    [Header("Healing")]
    public int healAmount;

    public override void Use() {
        if (GameManager.Instance.playerHealth != null) {
            GameManager.Instance.playerHealth.Heal(healAmount);
            Debug.Log($"Healed {healAmount} HP with {itemName}");
        }
        else {
            Debug.LogWarning("PlayerHealth not assigned in GameManager!");
        }
    }

}