using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [Header("Hitbox Settings")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float hitboxDuration = 0.10f;
    [SerializeField] public Transform hitboxSpawn;

    [Header("Weapon Setup")]
    [SerializeField] private WeaponStats currentWeapon;

    public PlayerStats playerStats;

    private void Awake() {
        
    }

    /// <summary>
    /// Spawns the attack hitbox. Called during attack animation window.
    /// </summary>
    public void PerformAttack() {

        Debug.Log("🔥 PerformAttack() called");

        if (hitboxPrefab == null || hitboxSpawn == null || currentWeapon == null) {
            Debug.LogWarning("Missing hitbox prefab, spawn transform, or weapon.");
            return;
        }

        GameObject go = Instantiate(hitboxPrefab, hitboxSpawn.position, hitboxSpawn.rotation);
        HitBox hb = go.GetComponentInChildren<HitBox>();

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
