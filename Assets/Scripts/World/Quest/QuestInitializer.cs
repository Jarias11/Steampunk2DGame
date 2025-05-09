using UnityEngine;

public class QuestInitializer : MonoBehaviour {
    public string startingQuestID;

    void Start() {
        var tracker = GameManager.Instance.playerQuestTracker;
        var data = QuestDatabase.Instance.GetQuestById(startingQuestID);

        if (data == null) {
            Debug.LogError($"❌ Quest '{startingQuestID}' not found in database!");
            return;
        }

        // Only start if not already active
        if (!tracker.activeQuests.Exists(q => q.questID == data.questID)) {
            var newQuest = new Quest {
                questID = data.questID,
                title = data.title,
                description = data.description,
                isActive = true,
                objectives = data.objectives.ConvertAll(o => new QuestObjective {
                    type = o.type,
                    targetId = o.targetId,
                    requiredAmount = o.requiredAmount,
                    currentAmount = 0
                })
            };

            tracker.StartQuest(newQuest);
            Debug.Log($"✅ Started quest: {newQuest.title}");
        }
    }
}
