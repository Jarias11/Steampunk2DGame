using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialItem : Item {
    public override void Use() {
        Debug.Log($"{itemName} is a material and cannot be used directly.");
        // Optional: show a tooltip or UI feedback
    }
}