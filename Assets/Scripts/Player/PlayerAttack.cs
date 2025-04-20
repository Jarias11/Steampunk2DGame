using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float hitboxDuration = 0.10f;
    [SerializeField] private Transform hitboxSpawn;
    
    //temp
    [SerializeField] private WeaponStats currentWeapon;
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;

    private PlayerStats playerStats;
    //private WeaponStats currentWeapon;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        ///currentWeapon = GetComponent<Inventory>().equppiedWeapon;
    }
    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
            SpawnHitbox();
        Debug.Log("Player Died!");
    }
    private void SpawnHitbox()
    {
        GameObject go = Instantiate(hitboxPrefab, hitboxSpawn.position, hitboxSpawn.rotation);
        go.GetComponent<HitBox>().Init(playerStats, currentWeapon, hitboxDuration);
    }
}
