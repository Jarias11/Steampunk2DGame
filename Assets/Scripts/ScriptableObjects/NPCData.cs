using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "Game Data/NPC")]
public class NPCData : ScriptableObject {
    public string npcName;
    public Sprite portrait;

    [Header("Dialogue Profile")]
    public NPCDialogueData dialogueAsset; // New field

    [Header("Meta Info")]
    public bool isRomanceable;
    public int age;
    public string favoriteItem;
}