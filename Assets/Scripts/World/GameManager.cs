using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [Header("Defaults")]
    public Sprite defaultNPCPortrait;

    [Header("Player References")]
    public Inventory playerInventory;
    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;
    public ArmorHolder playerArmorHolder;

    [Header("Managers")]
    public SaveManager saveManager;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}