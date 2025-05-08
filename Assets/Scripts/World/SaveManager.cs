using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SavedItemSlot {
    public int slotIndex;
    public string itemID;
    public int count;
}

[System.Serializable]
public class QuestSaveData {
    public string questId;
    public bool isActive;
    public List<int> objectiveProgress;
}



[System.Serializable]
public class SaveData {
    public float[] playerPosition;
    public int playerHealth;
    public GameTimeData gameTimeData;

    public List<SavedItemSlot> inventory;
    public List<WorldObjectData> worldObjects;
    public List<QuestSaveData> questProgress;
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
        data.playerPosition = new float[] { pos.x, pos.y };
        data.playerHealth = ph.GetCurrentHealth();
        data.gameTimeData = GameTime.Instance.GetTimeData();
        data.inventory = GameManager.Instance.playerInventory.ToSavedData();

        // Save active quests
        var tracker = GameManager.Instance.playerQuestTracker;
        data.questProgress = new List<QuestSaveData>();
        foreach (var quest in tracker.activeQuests) {
            data.questProgress.Add(new QuestSaveData {
                questId = quest.questID,
                isActive = quest.isActive,
                objectiveProgress = quest.objectives.Select(o => o.currentAmount).ToList()
            });
        }

        // Save destructible world objects
        var allBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        var destructibles = new List<IDestructibleWorldObject>();
        foreach (var mb in allBehaviours) {
            if (mb is IDestructibleWorldObject dwo)
                destructibles.Add(dwo);
        }

        data.worldObjects = destructibles.Select(d => d.GetSaveData()).ToList();

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

        var tracker = GameManager.Instance.playerQuestTracker;
        tracker.activeQuests.Clear(); // Optional: reset existing quests

        foreach (var qsd in data.questProgress) {
            var questData = QuestDatabase.Instance.GetQuestById(qsd.questId);
            if (questData == null) {
                Debug.LogWarning($"⚠️ Quest data not found for ID: {qsd.questId}");
                continue;
            }

            Quest loadedQuest = new Quest {
                questID = questData.questID,
                title = questData.title,
                description = questData.description,
                isActive = qsd.isActive,
                objectives = questData.objectives.Select((o, i) => new QuestObjective {
                    type = o.type,
                    targetId = o.targetId,
                    requiredAmount = o.requiredAmount,
                    currentAmount = (i < qsd.objectiveProgress.Count) ? qsd.objectiveProgress[i] : 0
                }).ToList()
            };

            tracker.activeQuests.Add(loadedQuest);
        }

        // ✅ Recalculate quest progress based on inventory after loading quests
        tracker.EvaluateCollectItemObjectives(GameManager.Instance.playerInventory);





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


