using UnityEngine;

public class NPCTalkState : NPCState {
    public override void Enter(NPCController npc) {
        npc.StopMoving();
        npc.ResetAnimation();
        npc.IsInDialogue = true;
    }

    public override void Update(NPCController npc) {
    }

    public override void Exit(NPCController npc) {
        
    }
}
