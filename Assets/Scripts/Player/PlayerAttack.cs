using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [Header("Hitbox Settings")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float hitboxDuration = 0.10f;
    [SerializeField] public Transform hitboxSpawn;

    [Header("Weapon Setup")]
    [SerializeField] private WeaponStats currentWeapon;

    private PlayerStats playerStats;

    private void Awake() {

    }

    /// <summary>
    /// Spawns the attack hitbox. Called during attack animation window.
    /// </summary>
    public void PerformAttack() {
        if (hitboxPrefab == null || hitboxSpawn == null || currentWeapon == null) {
            Debug.LogWarning("Missing hitbox prefab, spawn transform, or weapon.");
            return;
        }

        GameObject go = Instantiate(hitboxPrefab, hitboxSpawn.position, hitboxSpawn.rotation);
        HitBox hb = go.GetComponent<HitBox>();

        if (hb != null) {
            hb.Init(playerStats, currentWeapon, hitboxDuration);
        }
    }

    /// <summary>
    /// Used for updating the equipped weapon at runtime (optional).
    /// </summary>
    public void SetWeapon(WeaponStats newWeapon) {
        currentWeapon = newWeapon;
    }
}
