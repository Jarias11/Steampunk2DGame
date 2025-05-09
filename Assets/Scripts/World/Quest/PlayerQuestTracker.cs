using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerQuestTracker : MonoBehaviour {
    public List<Quest> activeQuests = new();
    //public List<Quest> completedQuests = new List<Quest>();

    public void StartQuest(Quest quest) {
        if (!activeQuests.Exists(q => q.questID == quest.questID)) {
            quest.isActive = true;
            activeQuests.Add(quest);
        }
    }

    public void UpdateObjective(ObjectiveType type, string targetId, int amount) {
        foreach (var quest in activeQuests) {
            if (!quest.isActive || quest.IsCompleted) continue;
            foreach (var obj in quest.objectives) {
                if (obj.type == type && obj.targetId == targetId && !obj.IsComplete) {
                    obj.currentAmount += amount;
                    if (obj.IsComplete) {
                        Debug.Log($"Objective {obj.targetId} of quest {quest.title} is complete!");
                        break; // Exit the loop if the objective is complete
                    }
                }
            }
        }
    }
    public bool IsQuestCompleted(string questID) =>
       activeQuests.Find(q => q.questID == questID)?.IsCompleted == true;


    public Quest CreateQuestInstance(QuestData data) {
        return new Quest {
            questID = data.questID,
            title = data.title,
            description = data.description,
            objectives = data.objectives.Select(o => new QuestObjective {
                type = o.type,
                targetId = o.targetId,
                requiredAmount = o.requiredAmount,
                currentAmount = 0
            }).ToList(),
            isActive = false
        };
    }
    public void EvaluateCollectItemObjectives(Inventory inventory) {
        foreach (var quest in activeQuests) {
            if (!quest.isActive || quest.IsCompleted) continue;

            foreach (var obj in quest.objectives) {
                if (obj.type != ObjectiveType.CollectItem || obj.IsComplete) continue;

                int totalInInventory = inventory.slots
                    .Where(s => s.item != null && s.item.itemID == obj.targetId)
                    .Sum(s => s.count);

                obj.currentAmount = Mathf.Min(totalInInventory, obj.requiredAmount);

                if (obj.IsComplete) {
                    Debug.Log($"✅ Objective complete: {obj.targetId} for quest {quest.title}");
                }
            }
        }
    }



}
