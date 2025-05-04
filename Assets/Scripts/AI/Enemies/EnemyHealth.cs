using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] private int maxHP = 40;
    private Health health;

    public void Awake() {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.SetMaxHealth(maxHP);
            health.Died += Die;
        }
    }    
    public void Die() { 
        Destroy(gameObject);
    }
}
