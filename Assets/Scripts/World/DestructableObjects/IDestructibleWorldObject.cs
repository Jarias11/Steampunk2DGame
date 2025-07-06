using UnityEngine;
public interface IDestructibleWorldObject {
    string GetID();                   // Unique type ID (e.g., "tree_oak", "rock_big")
    Vector2 GetPosition();
    bool IsDestroyed();
    WorldObjectData GetSaveData();
    void LoadFromData(WorldObjectData data);
}