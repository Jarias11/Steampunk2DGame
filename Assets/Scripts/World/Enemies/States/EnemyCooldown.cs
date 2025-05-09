using UnityEngine;

public class EnemyCooldown : EnemyBase {
    private float timer;

    public override void EnterState(EnemySM enemy) {
        timer = enemy.cooldownTime;
        enemy.Agent.isStopped = true;
    }

    public override void UpdateState(EnemySM enemy) {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        float dist = Vector2.Distance(enemy.transform.position, enemy.Player.position);

        if (enemy.canSeePlayer()) {
            if (dist <= enemy.attackRange)
                enemy.SwitchState(new EnemyWindUp());
            else
                enemy.SwitchState(new EnemyChase());
        }
        else
            enemy.LostPlayer(enemy.Player.position);
    }

    public override void ExitState(EnemySM enemy) {
        enemy.Agent.isStopped = false;
    }


}
