using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private PlayerStats stats;
    private Health health;
    public bool isDead;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        health = GetComponent<Health>();
        if (health != null) {
            health.SetMaxHealth(stats.maxHealth);
            health.HealthChanged += OnHealthChanged;
            health.Died += OnDeath;
        }
        ;
    }
    private void Start() {
        OnHealthChanged(health.CurrentHealth, stats.maxHealth);
    }


    public void Update() {
        if (Input.GetKeyDown(KeyCode.H)) health.TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.J)) health.Heal(5);
    }

    private void OnDeath() {
        isDead = true;
        Debug.Log("Player Died!");
    }

    public int GetCurrentHealth() => health.CurrentHealth;

    public void SetCurrentHealth(int val) => health.SetCurrentHealth(val);
    
    
    private void OnHealthChanged(int current, int max) {
        UIController.Instance.UpdateHealth(current, max);
    }
}

