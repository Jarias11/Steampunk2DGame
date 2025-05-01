using UnityEngine;
using System.Collections;

public class EnemyWindUp : EnemyBase {
    private bool facingRight;
    public override void EnterState(EnemySM enemy) {
        enemy.Agent.isStopped = true;
        facingRight = enemy.spriteRenderer.flipX;
        enemy.StartCoroutine(WindUpCoroutine(enemy));
    }

    private IEnumerator WindUpCoroutine(EnemySM enemy) {
        yield return new WaitForSeconds(enemy.windUpTime);
        enemy.SwitchState(new EnemyAttack(facingRight));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    public override void UpdateState(EnemySM enemy) {

    }
    public override void ExitState(EnemySM enemy) {
    }
}
