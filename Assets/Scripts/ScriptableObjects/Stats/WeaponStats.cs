using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Stats/Weapon")]
public class WeaponStats : ScriptableObject
{
    public string weaponName;

    [Header("Damage")]
    public int baseDamage = 10;
    public float critBonus = 0.25f;

    [Header("Damage Type (Optional)")]
    public DamageType damageType;

    [Header("Knockback (Optional)")]
    public float knockbackForce = 0f;

    [Header("Visuals")]
    public GameObject attackEffectPrefab;
}
