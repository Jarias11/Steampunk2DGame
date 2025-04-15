using UnityEngine;

/// <summary>
///  Trigger collider that lives for a fixed duration or until it
///  touches the player, then notifies its owning EnemyAttack state.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyHitbox : MonoBehaviour
{
    private float life;                    // seconds remaining
    private EnemyAttack owner;             // callback target

    /* ─────────────────────────────  Runtime setup  ───────────────────────────── */
    public void Init(EnemyAttack owner, float duration)
    {
        this.owner = owner;
        life       = duration;

        /* Ensure we’re a trigger so we don’t push the player around */
        GetComponent<Collider2D>().isTrigger = true;
    }

    /* ─────────────────────────────  Lifetime timer  ─────────────────────────── */
    private void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0f)
            EndHitbox();
    }

    /* ─────────────────────────────  Collision  ──────────────────────────────── */
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            /* 1️⃣  Deal damage (simple example) */
            var hp = col.GetComponent<PlayerHealth>();
            if (hp != null)
                hp.Damage(10);                 // TODO: expose damage in Inspector

            /* 2️⃣  Notify the owning state so it can exit early */
            owner?.OnHitboxEnd();

            /* 3️⃣  Destroy this hitbox */
            Destroy(gameObject);
        }
    }

    /* ─────────────────────────────  Helpers  ─────────────────────────────────── */
    private void EndHitbox()
    {
        owner?.OnHitboxEnd();
        Destroy(gameObject);
    }
}
