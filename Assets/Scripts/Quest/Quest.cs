using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public string questID;
    public string title;
    public string description;
    public List<QuestObjective> objectives;
    public bool isActive;
    public bool IsCompleted => objectives.TrueForAll(obj => obj.IsComplete);

    
}
