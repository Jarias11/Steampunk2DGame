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

    private void Awake() {
        Debug.Log("✅ HitBox Awake on " + gameObject.name);
    }

    private void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log($"[HitBox] Hit: {other.name}");

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null || other.gameObject == this.gameObject) return;

        ArmorStats defenderArmor = null;
        ArmorHolder armorHolder = other.GetComponent<ArmorHolder>();
        if (armorHolder != null) {
            defenderArmor = armorHolder.Stats;
        }

        int finalDamage = DamageResolver.Resolve(attackerStats, weaponStats, defenderArmor);
        Debug.Log($"[HitBox] {other.name} takes {finalDamage} damage");

        damageable.TakeDamage(finalDamage);
        // Destroy(gameObject); // if you want 1-hit-per-hitbox behavior
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}
