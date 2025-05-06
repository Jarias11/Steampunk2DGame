using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SavedItemSlot {
    public int slotIndex;
    public string itemID;
    public int count;
}

[System.Serializable]
public class SaveData {
    public float[] playerPosition;
    public int playerHealth;
    public GameTimeData gameTimeData;

    public List<SavedItemSlot> inventory;
    public List<WorldObjectData> worldObjects = new();
}




public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;
    private string savePath;
    private SaveData currentSaveData;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            savePath = Application.persistentDataPath + "/save.json";
        }
        else Destroy(gameObject);

    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            SaveManager.Instance.SaveGame(FindFirstObjectByType<PlayerMovement>(), FindFirstObjectByType<PlayerHealth>());

        if (Input.GetKeyDown(KeyCode.O))
            SaveManager.Instance.LoadGame(FindFirstObjectByType<PlayerMovement>(), FindFirstObjectByType<PlayerHealth>());
    }
    public void SaveGame(PlayerMovement pm, PlayerHealth ph) {
        var data = currentSaveData ?? new SaveData();
        Vector2 pos = pm.transform.position;
        data.playerPosition = new float[] { pos.x, pos.y, };
        data.playerHealth = ph.GetCurrentHealth();
        data.gameTimeData = GameTime.Instance.GetTimeData();
        data.inventory = GameManager.Instance.playerInventory.ToSavedData();


        // Save destructible world objects
        var allBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        var destructibles = new List<IDestructibleWorldObject>();

        foreach (var mb in allBehaviours) {
            if (mb is IDestructibleWorldObject dwo)
                destructibles.Add(dwo);
        }




        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        currentSaveData = data;

        Debug.Log("Game Saved to: " + savePath);
    }
    public void LoadGame(PlayerMovement playerMovement, PlayerHealth playerHealth) {
        if (!File.Exists(savePath)) {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        currentSaveData = data;


        if (data.worldObjects == null || data.worldObjects.Count == 0) {
            Debug.Log("🌱 No world object data — generating new world...");
            FindFirstObjectByType<WorldObjectSpawner>()?.SpawnInitialObjects();
            SaveGame(playerMovement, playerHealth); // Save the new world state immediately
            return; // Stop here since we've already saved + will load next time
        }



        Vector2 pos = new Vector2(data.playerPosition[0], data.playerPosition[1]);
        playerMovement.transform.position = pos;
        playerHealth.SetCurrentHealth(data.playerHealth);
        GameTime.Instance.SetTimeData(data.gameTimeData);
        GameManager.Instance.playerInventory.LoadFromSavedData(data.inventory);




        // 🌱 Load world objects
        foreach (var objData in data.worldObjects) {
            if (objData.isDestroyed) continue;

            GameObject prefab = WorldObjectDatabase.Instance.GetPrefabByID(objData.prefabID);
            if (prefab == null) {
                Debug.LogWarning($"Prefab not found for ID: {objData.prefabID}");
                continue;
            }

            GameObject go = Instantiate(prefab, objData.position, Quaternion.identity);
            if (go.TryGetComponent<IDestructibleWorldObject>(out var destructible)) {
                destructible.LoadFromData(objData);
            }
        }
        currentSaveData = data;

        Debug.Log("Game Loaded.");
    }



    public void RegisterWorldObject(WorldObjectData data) {
        if (!currentSaveData.worldObjects.Contains(data)) // optional deduplication
        {
            currentSaveData.worldObjects.Add(data);
        }
        else
            Debug.LogError("❌ Tried to register object but SaveData is null.");
        return;
    }
    public void MarkWorldObjectDestroyed(WorldObjectData obj) {
        if (currentSaveData == null || currentSaveData.worldObjects == null)
            return;

        foreach (var existing in currentSaveData.worldObjects) {
            if (existing.prefabID == obj.prefabID && existing.position == obj.position) {
                existing.isDestroyed = true;
                return;
            }
        }

        Debug.LogWarning("⚠️ Tried to mark destroyed object but couldn't find a match.");
    }

}


