using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour {
    public static TooltipController Instance;

    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform tooltipPanel;

    private void Awake() {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string message, RectTransform slotTransform) {
        tooltipText.text = message;
        tooltipPanel.position = slotTransform.position + new Vector3(30f, -20f, 0); // offset above slot
        tooltipPanel.gameObject.SetActive(true);
    }

    public void HideTooltip() {
        tooltipPanel.gameObject.SetActive(false);
    }
}