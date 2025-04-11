using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "Game Data/NPC")]
public class NPCData : ScriptableObject{
    public string npcName;
    public Sprite portrait;
    [TextArea(3,6)] public string [] dialogueLines;
    public bool isRomanceable;
    public int age;
    public string favoriteItem;
}