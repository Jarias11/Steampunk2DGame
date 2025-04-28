using UnityEngine;

public class EnemyDataProvider : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private WeaponStats weapon;

    public EnemyStats Stats => stats;
    public WeaponStats Weapon => weapon;
}