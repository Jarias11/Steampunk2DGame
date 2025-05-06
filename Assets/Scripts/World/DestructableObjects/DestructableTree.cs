using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ItemDropper))]
public class DestructableTree : MonoBehaviour, IDestructibleWorldObject {
    [SerializeField] private string prefabID = "tree";
    private bool isDestroyed = false;

    private Health health;
    private ItemDropper itemDropper;

    private void Awake() {
        health = GetComponent<Health>();
        itemDropper = GetComponent<ItemDropper>();

        if (health != null) {
            health.Died += OnHarvested;
        }
    }

    private void OnHarvested() {
        isDestroyed = true;
        itemDropper?.DropItems();



        // ✅ Mark as destroyed in save data
        var data = GetSaveData(); // Get the current state
        data.isDestroyed = true;

        SaveManager.Instance.MarkWorldObjectDestroyed(data); // Add this method below
        Destroy(gameObject);
    }

    // Interface implementation
    public string GetID() => prefabID;
    public Vector2 GetPosition() => transform.position;
    public bool IsDestroyed() => isDestroyed;

    public WorldObjectData GetSaveData() {
        return new WorldObjectData {
            id = "tree",                // category or system type
            prefabID = prefabID,
            position = transform.position,
            isDestroyed = isDestroyed,
            growthStage = 2 // fully grown for now
        };
    }

    public void LoadFromData(WorldObjectData data) {
        transform.position = data.position;
        isDestroyed = data.isDestroyed;
        // Optionally update visuals based on growthStage
    }
}