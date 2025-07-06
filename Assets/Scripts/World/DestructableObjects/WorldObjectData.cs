using UnityEngine;

[System.Serializable]
public class WorldObjectData
{
    public string id;               // "tree", "boulder", "bush", etc.
    public string prefabID;         // e.g., "oak_tree_1", "rock_large"
    public Vector2 position;        // Where it exists in the world
    public bool isDestroyed;        // Has it been cut/mined/etc.?
    public int growthStage;         // Optional: useful for plants or crops

    // Optional: more fields you might add in the future
    // public bool isPlayerPlaced;
    // public string customName;
    // public Vector2 facingDirection;
}