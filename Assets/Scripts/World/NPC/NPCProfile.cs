using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "NPC/NPC Profile")]
public class NPCProfile : ScriptableObject {
    [Header("Identity")]
    public string npcName;
    public Sprite portrait;
    public bool isRomanceable;
    public int age;
    public string favoriteItem;

    [Header("Dialogue States")]
    public List<string> initialEncounterLines;
    public List<string> questAcceptedLines;
    public List<string> inProgressLines;
    public List<string> questCompletedLines;
    public List<string> postQuestLines;

    [Header("Quest Info")]
    public string linkedQuestID;
}
