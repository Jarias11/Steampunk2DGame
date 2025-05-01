using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponItem : Item {
    [Header("Weapon Stats")]
    public WeaponStats weaponStats;

    public override void Use() {
        if (GameManager.Instance.playerAttack != null) {
            GameManager.Instance.playerAttack.SetWeapon(weaponStats);
            Debug.Log($"Equipped weapon: {itemName}");
        }
        else {
            Debug.LogWarning("PlayerAttack not assigned in GameManager!");
        }
    }
}