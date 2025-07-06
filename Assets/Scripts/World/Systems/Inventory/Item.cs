using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject {
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public Sprite worldIcon;
    public ItemRarity rarity;
    public bool isStackable = false;
    public int maxStack = 1;
    [HideInInspector] public string itemID; // Serialized, but not editable

#if UNITY_EDITOR
    private void OnValidate() {
        if (string.IsNullOrEmpty(itemID)) {
            itemID = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public abstract void Use(); // Every item must define how it is used
}
public enum ItemType {
    Weapon,
    Armor,
    Consumable,
    KeyItem,
    Material,
    QuestItem
}

public enum ItemRarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}