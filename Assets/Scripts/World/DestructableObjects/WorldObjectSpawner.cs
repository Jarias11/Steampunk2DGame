using UnityEngine;
using System.Collections.Generic;

public class WorldObjectSpawner : MonoBehaviour {
    [Header("Spawn Settings")]
    public List<string> spawnablePrefabIDs; // Must match IDs in WorldObjectDatabase



    public float areaX = -20f;
    public float areaY = -20f;
    public float areaWidth = 40f;
    public float areaHeight = 40f;



    public int objectCount = 30;
    public float minDistance = 2f;


    private Rect GetSpawnArea() {
        return new Rect(areaX, areaY, areaWidth, areaHeight);
    }

    public void SpawnInitialObjects() {
        Rect spawnArea = GetSpawnArea();
        var usedPositions = new List<Vector2>();
        Debug.Log("🌲 Starting world object spawning...");
        Debug.Log($"Spawnable IDs count: {spawnablePrefabIDs.Count}");

        for (int i = 0; i < objectCount * 10 && usedPositions.Count < objectCount; i++) {
            Vector2 randomPos = new Vector2(
                Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax)
            );

            if (IsTooClose(randomPos, usedPositions)) continue;

            // Choose a prefab ID randomly
            string prefabID = spawnablePrefabIDs[Random.Range(0, spawnablePrefabIDs.Count)];
            GameObject prefab = WorldObjectDatabase.Instance.GetPrefabByID(prefabID);
            if (prefab == null) {
                Debug.LogWarning($"Prefab ID not found: {prefabID}");
                continue;
            }

            GameObject go = Instantiate(prefab, randomPos, Quaternion.identity);
            // ✅ Attach YSort if it doesn't exist
            if (!go.TryGetComponent<YSort>(out _)) {
                go.AddComponent<YSort>();
            }
            if (go.TryGetComponent<IDestructibleWorldObject>(out var destructible)) {
                var data = destructible.GetSaveData();
                SaveManager.Instance.RegisterWorldObject(data); // We'll create this
                usedPositions.Add(randomPos);
            }
        }

        Debug.Log($"🌱 Spawned {usedPositions.Count} world objects");
    }

    private bool IsTooClose(Vector2 pos, List<Vector2> existing) {
        foreach (var other in existing) {
            if (Vector2.Distance(pos, other) < minDistance)
                return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected() {
        Rect area = GetSpawnArea();

        Gizmos.color = Color.red;

        Vector3 bottomLeft = new Vector3(area.xMin, area.yMin, 0f);
        Vector3 bottomRight = new Vector3(area.xMax, area.yMin, 0f);
        Vector3 topRight = new Vector3(area.xMax, area.yMax, 0f);
        Vector3 topLeft = new Vector3(area.xMin, area.yMax, 0f);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}