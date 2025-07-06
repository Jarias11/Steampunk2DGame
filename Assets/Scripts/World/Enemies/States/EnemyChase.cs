using UnityEngine;

public class EnemyChase : EnemyBase {
    private const float REPATH_INTERVAL = 0.10F;
    private float repathTimer;

    public override void EnterState(EnemySM enemy) {
        enemy.Agent.isStopped = false;
        SetDestination(enemy);
        repathTimer = REPATH_INTERVAL;
    }

    public override void UpdateState(EnemySM enemy) {
        //if there is no player go to idle
        if (enemy.Player == null) {
            enemy.SwitchState(new EnemyIdle());
            return;
        }
        //if enemy cant see player run LostPlayer method
        if (!enemy.canSeePlayer()) {
            enemy.LostPlayer(enemy.Player.position);
            return;
        }
        //create dist vector between enemy and player and if close enough go to windup
        float dist = Vector2.Distance(enemy.transform.position, enemy.Player.position);
        if (dist < enemy.attackRange) {
            enemy.SwitchState(new EnemyWindUp());
            return;
        }
        //flips sprite
        enemy.flip(enemy.Player.position);//Flips sprite

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f) {
            SetDestination(enemy);
            repathTimer = REPATH_INTERVAL;
        }
    }
    public override void ExitState(EnemySM enemy) {
        enemy.Agent.isStopped = true;
    }
    private void SetDestination(EnemySM enemy) {
        enemy.Agent.SetDestination(enemy.Player.position);
    }

}
