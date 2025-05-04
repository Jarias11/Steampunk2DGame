using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public delegate void OnHealthChanged(int curren, int max);
    public event OnHealthChanged HealthChanged;
    public delegate void OnDeath();
    public event OnDeath Died;


    private void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        Debug.Log($"🌲 {gameObject.name} took {damage} damage!");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }
    public void Heal(int amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetCurrentHealth(int health) {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public void Die() {
        Died?.Invoke();
    }

    public void SetMaxHealth(int health) {
        maxHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

}
