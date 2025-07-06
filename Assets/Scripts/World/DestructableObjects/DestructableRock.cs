using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ItemDropper))]
public class DestructableRock : MonoBehaviour, IDestructibleWorldObject {
    [SerializeField] private string prefabID = "rock_small"; // or "rock_big"
    private bool isDestroyed = false;

    private Health health;
    private ItemDropper itemDropper;

    private void Awake() {
        health = GetComponent<Health>();
        itemDropper = GetComponent<ItemDropper>();

        if (health != null) {
            health.Died += OnMined;
        }
    }

    private void OnMined() {
        isDestroyed = true;
        itemDropper?.DropItems();

        var data = GetSaveData();
        data.isDestroyed = true;
        SaveManager.Instance.MarkWorldObjectDestroyed(data);

        Destroy(gameObject);
    }

    // Interface implementation
    public string GetID() => prefabID;
    public Vector2 GetPosition() => transform.position;
    public bool IsDestroyed() => isDestroyed;

    public WorldObjectData GetSaveData() {
        return new WorldObjectData {
            id = prefabID, // general type
            prefabID = prefabID,
            position = transform.position,
            isDestroyed = isDestroyed,
            growthStage = 0 // not used for rocks, but needed for now
        };
    }

    public void LoadFromData(WorldObjectData data) {
        transform.position = data.position;
        isDestroyed = data.isDestroyed;
        // You could change visuals here if desired
    }
}
