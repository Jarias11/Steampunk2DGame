using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC Dialogue Data")]
public class NPCDialogueData : ScriptableObject {
    public string npcName;
    public Sprite portrait;

    [Header("Dialogue States")]
    public List<string> initialEncounterLines;
    public List<string> questAcceptedLines;
    public List<string> inProgressLines;
    public List<string> questCompletedLines;
    public List<string> postQuestLines;

    [Header("Quest Info")]
    public string linkedQuestID;
}