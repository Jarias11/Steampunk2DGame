using UnityEngine;

public class EnemyReturnHome : EnemyBase {
    /* ──────────────────────────────  Tunables  ────────────────────────────── */
    private const float ARRIVAL_THRESHOLD = 0.15f;   // world‑unit tolerance

    /* ──────────────────────────────  Enter  ──────────────────────────────── */
    public override void EnterState(EnemySM enemy) {
        enemy.Agent.isStopped = false;
        enemy.Agent.SetDestination(enemy.spawnPos);
    }

    /* ──────────────────────────────  Update  ─────────────────────────────── */
    public override void UpdateState(EnemySM enemy) {
        /* 1️⃣  If the player is spotted on the way home, re‑engage */
        if (enemy.canSeePlayer()) {
            enemy.SwitchState(new EnemyChase());
            return;
        }

        /* 2️⃣  Check if we’ve reached the original spawn point */
        bool arrived = !enemy.Agent.pathPending &&
                       enemy.Agent.remainingDistance <= ARRIVAL_THRESHOLD;

        if (arrived) {
            enemy.SwitchState(new EnemyIdle());
        }
    }

    /* ──────────────────────────────  Exit  ───────────────────────────────── */
    public override void ExitState(EnemySM enemy) {
        enemy.Agent.isStopped = true;      // stand still once back home
    }
}
