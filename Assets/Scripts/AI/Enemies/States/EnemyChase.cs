using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyChase : EnemyBase
{

    public override void EnterState(EnemySM enemy)
    {
        enemy.Agent.isStopped = false;
    }

    public override void UpdateState(EnemySM enemy)
    {

        if (enemy.Player == null) return;

        float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

        enemy.flip(enemy.Player.position);//Flips sprite

        if (distance > enemy.chaseRange)
        {
            enemy.SwitchState(new EnemyIdle());
            return;
        }

        enemy.Agent.SetDestination(enemy.Player.position);
    }
    public override void ExitState(EnemySM enemy)
    {
        enemy.Agent.isStopped = true;
    }

}
