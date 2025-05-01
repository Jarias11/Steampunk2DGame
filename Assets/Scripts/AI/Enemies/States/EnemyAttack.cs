using UnityEngine;

public class EnemyAttack : EnemyBase {
    private readonly bool facingRight;
    private HitBox hitbox;
    private float timer;

    private EnemyStats enemyStats;
    private WeaponStats weaponStats;

    public EnemyAttack(bool facingRight) {
        this.facingRight = facingRight;
    }

    public override void EnterState(EnemySM enemy) {
        enemy.Agent.isStopped = true;

        // Setup direction and spawn position
        Vector2 forward = facingRight ? Vector2.right : Vector2.left;
        float offset = enemy.transform.localScale.x * 0.5f;
        Vector2 spawnPos = (Vector2)enemy.transform.position + forward * offset;

        // Create hitbox
        GameObject go = Object.Instantiate(enemy.hitboxPrefab, spawnPos, Quaternion.identity);

        // Get enemy stats and weapon from the state machine or another component
        enemyStats = enemy.GetComponent<EnemyDataProvider>()?.Stats;
        weaponStats = enemy.GetComponent<EnemyDataProvider>()?.Weapon;

        hitbox = go.GetComponent<HitBox>();
        if (hitbox != null && enemyStats != null && weaponStats != null) {
            hitbox.Init(enemyStats, weaponStats, enemy.hitboxTime);
        }
        else {
            Debug.LogWarning("Missing hitbox or stats on enemy attack!");
        }

        timer = enemy.hitboxTime;
    }

    public override void UpdateState(EnemySM enemy) {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            EndAttack(enemy);
        }
    }

    public override void ExitState(EnemySM enemy) {
        if (hitbox != null) {
            Object.Destroy(hitbox.gameObject);
        }
    }

    private void EndAttack(EnemySM enemy) {
        enemy.SwitchState(new EnemyCooldown());
    }
}
