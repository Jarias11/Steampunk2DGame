using UnityEngine;

public abstract class EnemyBase {
    public abstract void EnterState(EnemySM enemy);
    public abstract void UpdateState(EnemySM enemy);
    public abstract void ExitState(EnemySM enemy);
}
