using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class FarmTileManager : MonoBehaviour {
    public Tilemap interactableTilemap;

    private Dictionary<Vector3Int, FarmTileData> tileData = new Dictionary<Vector3Int, FarmTileData>();

    public static FarmTileManager Instance;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(gameObject);
        }
    }
    public FarmTileData GetTileData(Vector3Int pos) {
        if (!tileData.ContainsKey(pos))
            tileData[pos] = new FarmTileData();
        return tileData[pos];
    }
    public bool IsInInteractableRange(Vector3 playerWorldPos, Vector3Int tilePos) {
        Vector3Int playerTile = interactableTilemap.WorldToCell(playerWorldPos);
        Vector3Int diff = tilePos - playerTile;
        return (Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1);
    }
    public void TillTile(Vector3Int tilePos, Vector3 playerWorldPos) {
        if (!IsInInteractableRange(playerWorldPos, tilePos)) return;
        TileBase targetTile = interactableTilemap.GetTile(tilePos);

        if (targetTile == null) {
            Debug.Log("❌ No tile here to till.");
            return;
        }
        FarmTileData data = GetTileData(tilePos);
        if (data.isTilled) {
            Debug.Log("⚠️ Already tilled.");
            return; // 🚫 Don't till again
        }
        data.isTilled = true;

        Debug.Log($"Tilled tile at {tilePos}");
    }
}
