using UnityEngine;

public class SeasonalLeaves : MonoBehaviour {
    [SerializeField] private SpriteRenderer leafRenderer;

    [Header("Seasonal Colors (Tint for Greyscale)")]
    [SerializeField] private Color springColor = Color.green;
    [SerializeField] private Color summerColor = new Color(0.4f, 0.8f, 0.3f);
    [SerializeField] private Color fallColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color winterColor = Color.white;

    private void Start() {
        GameTime.Instance.OnSeasonChanged += UpdateLeafColor;
        UpdateLeafColor(GameTime.Instance.currentSeason);
    }

    private void UpdateLeafColor(GameTime.Season season) {
        Color targetColor = season switch {
            GameTime.Season.Spring => springColor,
            GameTime.Season.Summer => summerColor,
            GameTime.Season.Fall => fallColor,
            GameTime.Season.Winter => winterColor,
            _ => Color.white
        };

        ApplyTint(targetColor);
    }

    private void ApplyTint(Color tint) {
        tint.a = 1f; // Force full opacity so sprite never disappears
        leafRenderer.color = tint;

        Debug.Log($"[SeasonalLeaves] Applied seasonal tint: {tint}");
    }
}