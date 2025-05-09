using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/QuestData")]
public class QuestData : ScriptableObject {
    public string questID;
    public string title;
    [TextArea] public string description;
    public List<QuestObjective> objectives;
}
