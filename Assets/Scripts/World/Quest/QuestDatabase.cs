using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestDatabase : MonoBehaviour {
    public static QuestDatabase Instance;

    public List<QuestData> allQuests;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public QuestData GetQuestById(string id) {
        return allQuests.FirstOrDefault(q => q.questID == id);
    }
}