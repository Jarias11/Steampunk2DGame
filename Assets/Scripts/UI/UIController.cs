using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateHealth(int current, int max)
    {
        healthText.text = $"Health: {current} / {max}";
    }
}
