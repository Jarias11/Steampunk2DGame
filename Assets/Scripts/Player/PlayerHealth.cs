using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int currentHealth;
    public bool isDead;
    [SerializeField] private int startingHealth;
    //creating an event ... public event Action<int,int> OnHealthChanged;

    void Start()
    {
        currentHealth = PlayerStats.maxHealth;
        UIController.Instance.UpdateHealth(currentHealth, PlayerStats.maxHealth);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, PlayerStats.maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, PlayerStats.maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int healAmount)
    {
        Debug.Log("Current Health: " + currentHealth);
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, PlayerStats.maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, PlayerStats.maxHealth);
    }
    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
    }
}

