using UnityEngine;

public enum ObjectiveType {
    CollectItem,
    TalkToNPC,
    VisitLocation
}

[System.Serializable]
public class QuestObjective {
    public ObjectiveType type;
    public string targetId; // item ID, NPC name, or location ID
    public int requiredAmount = 1;
    public int currentAmount = 0;
    public bool IsComplete => currentAmount >= requiredAmount;
}