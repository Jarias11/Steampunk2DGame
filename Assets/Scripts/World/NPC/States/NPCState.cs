using UnityEngine;

public abstract class NPCState {
    public abstract void Enter(NPCController npc);
    public abstract void Update(NPCController npc);
    public abstract void Exit(NPCController npc);
}
