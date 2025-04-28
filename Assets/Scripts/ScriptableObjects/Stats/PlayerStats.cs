using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Stats/Player")]
public class PlayerStats : ScriptableObject
{
    [Header("Health & Defense")]
    public int maxHealth = 100;
    public int defense = 10;

    [Header("Offense")]
    public int baseDamage = 10;
    public int power = 100;

    [Header("Criticals")]
    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;

    [Header("Speed & Misc")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
}
