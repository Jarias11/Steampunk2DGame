using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Stats/Armor")]
public class ArmorStats : ScriptableObject {
    public string armorName;

    [Header("Damage Reduction")]
    public int flatReduction = 2;
    public float percentReduction = 0.2f;

    [Header("Resistances (Optional)")]
    public DamageType[] resistTypes;
}
