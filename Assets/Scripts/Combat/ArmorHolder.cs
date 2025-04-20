using UnityEngine;

public class ArmorHolder : MonoBehaviour
{
    [SerializeField] private ArmorStats armorStats;

    /// <summary>
    /// Gets the equipped armor stats for this character.
    /// </summary>
    public ArmorStats Stats => armorStats;

    // Optional: expose a setter for dynamic equipping
    public void SetArmor(ArmorStats newArmor)
    {
        armorStats = newArmor;
    }
}