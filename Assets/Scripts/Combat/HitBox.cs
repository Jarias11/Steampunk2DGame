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
    private GameObject attacker;
    public void Init(PlayerStats attackerStats, WeaponStats weapon, float duration, GameObject attackerGO) {
        this.attackerStats = attackerStats;
        this.weaponStats = weapon;
        this.lifeTime = duration;
        this.attacker = attackerGO;
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

        if (other.gameObject == this.gameObject || other.gameObject == attacker)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;

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

        Collider2D col = GetComponent<Collider2D>();
        if (col is CapsuleCollider2D cap) {
            Vector3 size = new Vector3(cap.size.x, cap.size.y, 0);
            Vector3 pos = cap.transform.position + (Vector3)cap.offset;
            Gizmos.matrix = cap.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(cap.offset, size); // use DrawWireCube since Gizmos lacks capsule drawing
        }
        else if (col is BoxCollider2D box) {
            Vector3 size = new Vector3(box.size.x, box.size.y, 0);
            Vector3 pos = box.transform.position + (Vector3)box.offset;
            Gizmos.matrix = box.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(box.offset, size);
        }
    }
}
