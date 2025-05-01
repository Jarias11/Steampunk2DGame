using UnityEngine;

/// <summary>
/// A temporary damage zone spawned during attacks. Can be used by both players and enemies.
/// </summary>
public class HitBox : MonoBehaviour {
    private PlayerStats attackerStats;
    private WeaponStats weaponStats;
    private float lifeTime;

    /// <summary>
    /// Initializes the hitbox with attacker + weapon data and how long it should last.
    /// </summary>
    public void Init(PlayerStats attacker, WeaponStats weapon, float duration) {
        attackerStats = attacker;
        weaponStats = weapon;
        lifeTime = duration;
    }

    private void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // 1. Does the thing we hit take damage?
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null || other.gameObject == this.gameObject) return;

        // 2. Get armor, if any
        ArmorStats defenderArmor = null;
        ArmorHolder armorHolder = other.GetComponent<ArmorHolder>();
        if (armorHolder != null) {
            defenderArmor = armorHolder.Stats;
        }

        // 3. Resolve final damage
        int finalDamage = DamageResolver.Resolve(attackerStats, weaponStats, defenderArmor);

        // 4. Apply damage
        damageable.TakeDamage(finalDamage);

        // Optional: destroy hitbox on first contact
        // Destroy(gameObject);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}
