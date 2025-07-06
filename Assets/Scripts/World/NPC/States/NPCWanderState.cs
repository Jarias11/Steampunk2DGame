using UnityEngine;


public class NPCWanderState : NPCState {
    private float waitTimer;
    private bool isWalking;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3[] directions = new Vector3[] {
        Vector3.down,
        Vector3.left,
        Vector3.up,
        Vector3.right
    };
    private int currentDir;
    private float moveSpeed = 1f;
    private float walkDistance = 5f;
    private float waitTime = 1f;

    public override void Enter(NPCController npc) {
        startPos = npc.transform.position;
        currentDir = Random.Range(0, directions.Length);
        SetNewTarget(npc);
        isWalking = true;
    }

    public override void Update(NPCController npc) {
        if (npc.IsInDialogue) return;
        if (isWalking) {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPos, moveSpeed * Time.deltaTime);
            npc.AnimateDirection(targetPos - npc.transform.position);

            if (Vector3.Distance(npc.transform.position, targetPos) < 0.01f) {
                isWalking = false;
                waitTimer = waitTime;
                npc.ResetAnimation();
            }
        }
        else {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) {
                isWalking = true;
                currentDir = (currentDir + 1) % directions.Length;
                startPos = npc.transform.position;
                SetNewTarget(npc);
            }
        }
    }

    public override void Exit(NPCController npc) {
        npc.ResetAnimation();
    }

    private void SetNewTarget(NPCController npc) {
        targetPos = startPos + directions[currentDir] * walkDistance;
    }
}