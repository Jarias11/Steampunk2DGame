using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [Header("Hitbox Settings")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float hitboxDuration = 0.10f;
    [SerializeField] public Transform hitboxSpawn;

    [Header("Weapon Setup")]
    [SerializeField] private WeaponStats currentWeapon;

    public PlayerStats playerStats;
    public Vector2 lastDirection = Vector2.right;

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

        // Get direction from PlayerMovement
        Vector2 dir = lastDirection.normalized;
        Vector3 offset = new Vector3(dir.x, dir.y) * 0.5f;

        // Get player's sprite height to adjust vertical spawn center
        offset += new Vector3(-0.5f, 2f);

        Vector3 spawnPos = transform.position + offset;

        GameObject go = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);
        HitBox hb = go.GetComponentInChildren<HitBox>();

        if (hb != null) {
            hb.Init(playerStats, currentWeapon, hitboxDuration, gameObject);
        }
    }

    /// <summary>
    /// Used for updating the equipped weapon at runtime (optional).
    /// </summary>
    public void SetWeapon(WeaponStats newWeapon) {
        currentWeapon = newWeapon;
    }
}
