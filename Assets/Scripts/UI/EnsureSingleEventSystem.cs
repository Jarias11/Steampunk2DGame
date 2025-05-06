using UnityEngine;
using UnityEngine.EventSystems;

public class EnsureSingleEventSystem : MonoBehaviour {
    void Awake() {
        // Use the new API for Unity 2023+ (no sorting)
        EventSystem[] systems = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        if (systems.Length > 1) {
            for (int i = 1; i < systems.Length; i++) {
                Destroy(systems[i].gameObject);
            }
        }
    }
}