using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour {
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate() {
        // Invert so lower Y renders on top
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + 500;
        Debug.Log($"{gameObject.name} sortingOrder = {sr.sortingOrder}");
    }
}
