using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Items/Food")]
public class FoodItem : Item {
    [Header("Healing")]
    public int healAmount;

    public override void Use() {
        var playerHealth = GameManager.Instance.playerHealth;

        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth adapter not assigned!");
            return;
        }

        var healthComponent = playerHealth.GetComponent<Health>();
        if (healthComponent == null) {
            Debug.LogWarning("No Health component found on player!");
            return;
        }

        healthComponent.Heal(healAmount);
        Debug.Log($"🍎 Healed player for {healAmount} HP.");
    }

}