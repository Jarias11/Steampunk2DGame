using Unity.VisualScripting;
using UnityEngine;

public class DamageResolver
{
    public static int Resolve(
        PlayerStats attackerStats, weaponStats weapon, ArmorStats defenderArmor
    ){
        float baseDmg = weapon.baseDamage + attackerStats.strength * 0.5f;
        float crit = Random.value<attackerStats.critChance ? 1.5f : 1f;
        float reduced = baseDmg * crit - defenderArmor.flatReduction;
        reduced *= 1f - defenderArmor.percentReduction;
        
        return Mathf.Max(1,Mathf.RoundToInt(reduced));  
    }
}
