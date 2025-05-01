using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorItem : Item
{
    [Header("Armor Stats")]
    public ArmorStats armorStats;

    public override void Use()
    {
        if (GameManager.Instance.playerArmorHolder != null)
        {
            GameManager.Instance.playerArmorHolder.SetArmor(armorStats);
            Debug.Log($"Equipped armor: {itemName}");
        }
        else
        {
            Debug.LogWarning("ArmorHolder not assigned in GameManager!");
        }
    }
}