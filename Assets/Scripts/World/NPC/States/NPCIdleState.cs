using UnityEngine;

public class NPCIdleState : NPCState {
    public override void Enter(NPCController npc) {
        npc.StopMoving();
    }

    public override void Update(NPCController npc) {
        // Do nothing (idle)
    }

    public override void Exit(NPCController npc) {
        // Optional: reset animation if needed
    }
}
