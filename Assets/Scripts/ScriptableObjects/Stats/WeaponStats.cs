using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Stats/Weapon")]
public class WeaponStats : ScriptableObject
{
    public string weaponName;
    public int baseDamage = 10;
    public float critBonus = 0.5f;
    //public DamageType damageType; // optional, for fire, ice, etc.
}
