using UnityEngine;

public class EnemyChase: EnemyBase{
    public override void EnterState(EnemySM enemy)
    {

    }

    public override void UpdateState(EnemySM enemy)
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
        if (distance > enemy.chaseRange)
        {
            enemy.SwitchState(new EnemyIdle());
            return;
        }
        Vector2 direction = (enemy.Player.position - enemy.transform.position).normalized;
        enemy.transform.position += (Vector3)(direction * enemy.moveSpeed * Time.deltaTime);
    }
    public override void ExitState(EnemySM enemy)
    {
        
    }

}
