using UnityEngine;

public class EnemyIdle: EnemyBase
{
    private float visionTimer = 0;
    private const float VISION_INTERVAL = 0.2f;


    public override void EnterState(EnemySM enemy){
        
    }
    
    public override void UpdateState(EnemySM enemy){
        visionTimer-= Time.deltaTime;
        if(visionTimer<=0f){
            if (enemy.canSeePlayer())
                enemy.SwitchState(new EnemyChase());
            visionTimer = VISION_INTERVAL;
        }
        
        
        
    }
    public override void ExitState(EnemySM enemy){

    }
}
