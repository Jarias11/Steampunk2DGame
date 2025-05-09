using UnityEngine;

public class EnemySearch : EnemyBase {
    /* ──────────────────────────────  Tunables  ────────────────────────────── */
    private const float ARRIVAL_THRESHOLD = 0.15f;   // world units
    private const float SEARCH_TIME_MAX = 2.0f;    // seconds to keep scanning
    private const float FLIP_INTERVAL = 0.30f;   // how fast to look L↔R

    /* ──────────────────────────────  State data  ──────────────────────────── */
    private readonly Vector3 targetPos;   // last‑seen player position
    private float searchTimer;
    private float flipTimer;

    /* Constructor receives the position cached in EnemySM.LostPlayer() */
    public EnemySearch(Vector3 lastSeenPos) {
        targetPos = lastSeenPos;
    }

    /* ──────────────────────────────  Enter  ──────────────────────────────── */
    public override void EnterState(EnemySM enemy) {
        enemy.Agent.isStopped = false;
        enemy.Agent.SetDestination(targetPos);

        searchTimer = SEARCH_TIME_MAX;
        flipTimer = FLIP_INTERVAL;
    }

    /* ──────────────────────────────  Update  ─────────────────────────────── */
    public override void UpdateState(EnemySM enemy) {
        /* 1️⃣  Regain vision while walking?  ➜  Chase */
        if (enemy.canSeePlayer()) {
            enemy.SwitchState(new EnemyChase());
            return;
        }

        /* 2️⃣  Have we reached the last‑seen spot? */
        bool arrived = !enemy.Agent.pathPending &&
                       enemy.Agent.remainingDistance <= ARRIVAL_THRESHOLD;

        if (!arrived) return;   // keep walking

        /* 3️⃣  Perform left/right scan */
        enemy.Agent.isStopped = true;      // stop moving during the scan
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f) {
            /* Scan timed out ➜ Return home */
            enemy.SwitchState(new EnemyReturnHome());
            return;
        }

        /* Flip sprite left ↔ right at a steady cadence */
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0f) {
            enemy.spriteRenderer.flipX = !enemy.spriteRenderer.flipX;
            flipTimer = FLIP_INTERVAL;
        }

        /* Check vision again after the flip */
        if (enemy.canSeePlayer()) {
            enemy.SwitchState(new EnemyChase());
        }
    }

    /* ──────────────────────────────  Exit  ───────────────────────────────── */
    public override void ExitState(EnemySM enemy) {
        enemy.Agent.isStopped = false;  // allow movement in next state
    }
}
