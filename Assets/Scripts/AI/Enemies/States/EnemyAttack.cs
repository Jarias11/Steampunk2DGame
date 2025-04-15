using UnityEngine;

public class EnemyAttack : EnemyBase
{
    private readonly bool facingRight;
    private EnemyHitbox hitbox;
    private float timer;

    public EnemyAttack(bool facingRight){
        this.facingRight = facingRight;
    }
    public override void EnterState(EnemySM enemy){
        enemy.Agent.isStopped = true;

        Vector2 forward = facingRight ? Vector2.right : Vector2.left;
        float offset = enemy.transform.localScale.x * 0.5f;
        Vector2 spawnPos = (Vector2)enemy.transform.position + forward * offset;

        GameObject go = Object.Instantiate(
            enemy.hitboxPrefab,spawnPos,Quaternion.identity
        );
        hitbox = go.GetComponent<EnemyHitbox>();
        hitbox.Init(this, enemy.hitboxTime);

        timer = enemy.hitboxTime;
    }

    public override void UpdateState(EnemySM enemy){
        timer -= Time.deltaTime;
        if(timer <= 0f)
            EndAttack(enemy);
    }
    public override void ExitState(EnemySM enemy){
        if(hitbox != null) Object.Destroy(hitbox.gameObject);
    }
    public void OnHitboxEnd(){
        timer = 0f;
    }
    private void EndAttack(EnemySM enemy){
        enemy.SwitchState(new EnemyCooldown());
    }

}
