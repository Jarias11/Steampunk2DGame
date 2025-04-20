using UnityEngine;

public static class DamageResolver
{
    /// <summary>
    /// Calculates final damage based on attacker stats, weapon, and defender armor.
    /// </summary>
    public static int Resolve(
        PlayerStats attackerStats,
        WeaponStats weapon,
        ArmorStats defenderArmor = null
    )
    {
        if (attackerStats == null || weapon == null)
        {
            Debug.LogWarning("Missing attacker or weapon data in DamageResolver.");
            return 0;
        }

        // 1. Base Damage Calculation
        float baseDamage = weapon.baseDamage + (attackerStats.power * 0.2f);

        // 2. Critical Hit Chance
        bool isCrit = Random.value < attackerStats.critChance;
        if (isCrit)
        {
            float totalCritMultiplier = attackerStats.critMultiplier + weapon.critBonus;
            baseDamage *= totalCritMultiplier;
            Debug.Log("💥 Critical Hit!");
        }

        // 3. Armor Reduction (if provided)
        if (defenderArmor != null)
        {
            // Optional: check for resistances
            if (defenderArmor.resistTypes != null &&
                System.Array.Exists(defenderArmor.resistTypes, t => t == weapon.damageType))
            {
                Debug.Log("🛡️ Damage resisted!");
                baseDamage *= 0.5f; // half damage from resisted types
            }

            baseDamage -= defenderArmor.flatReduction;
            baseDamage *= (1f - defenderArmor.percentReduction);
        }

        // 4. Clamp and return
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage));
        return finalDamage;
    }
}
