using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStats stats;
    private int currentHealth;
    public bool isDead;
    [SerializeField] private int startingHealth;
    //creating an event ... public event Action<int,int> OnHealthChanged;

    void Start()
    {
        currentHealth = stats.maxHealth;
        UIController.Instance.UpdateHealth(currentHealth, stats.maxHealth);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.J)) Heal(5);
    }
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Current Health: " + currentHealth);
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, stats.maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, stats.maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int healAmount)
    {
        Debug.Log("Current Health: " + currentHealth);
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, stats.maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, stats.maxHealth);
    }
    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, stats.maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, stats.maxHealth);
    }
}

