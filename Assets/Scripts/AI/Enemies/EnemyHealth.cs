using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]private int maxHP = 40;
    private int currentHP;

    public void Awake() => currentHP = maxHP;

    public void TakeDamage(int amount){
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }
    public void Die(){}
}
