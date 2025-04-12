using UnityEngine;

public class EnemyIdle: EnemyBase
{
    public override void EnterState(EnemySM enemy){
        
    }
    
    public override void UpdateState(EnemySM enemy){
        float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
        if (distance <= enemy.chaseRange){
            enemy.SwitchState(new EnemyChase());
        }   
    }
    public override void ExitState(EnemySM enemy){

    }
}
