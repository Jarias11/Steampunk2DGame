using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPCController : MonoBehaviour {
    private NPCState currentState;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public bool IsInDialogue { get; set; } = false;

    void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        GameTime.Instance.OnTimeChanged += EvaluateSchedule;
        EvaluateSchedule(GameTime.Instance.hour, GameTime.Instance.day, GameTime.Instance.month);
    }

    void OnDestroy() {
        if (GameTime.Instance != null)
            GameTime.Instance.OnTimeChanged -= EvaluateSchedule;
    }

    void Update() {
        currentState?.Update(this);
    }

    public void SwitchState(NPCState newState) {
        if (currentState != null) {
            Debug.Log($"🔄 Switching from {currentState.GetType().Name} to {newState.GetType().Name}");
            var prev = currentState;
            currentState = null;
            prev.Exit(this);
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void StopMoving() {
        // Could also stop nav agent or rigidbody if used
    }

    public void AnimateDirection(Vector3 dir) {
        ResetAnimation();
        dir = dir.normalized;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
            animator.SetBool("isWalkingRight", dir.x > 0);
            animator.SetBool("isWalkingLeft", dir.x < 0);
        }
        else {
            animator.SetBool("isWalkingUp", dir.y > 0);
            animator.SetBool("isWalkingDown", dir.y < 0);
        }
    }

    public void ResetAnimation() {
        animator.SetBool("isWalkingDown", false);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingUp", false);
        animator.SetBool("isWalkingRight", false);
    }

    private void EvaluateSchedule(int hour, int day, int month) {
        if (IsInDialogue) return;

        if (hour >= 6 && hour < 20) {
            SwitchState(new NPCWanderState());
        }
        else {
            SwitchState(new NPCIdleState());
        }
    }
    public void TryEvaluateSchedule() {
        EvaluateSchedule(GameTime.Instance.hour, GameTime.Instance.day, GameTime.Instance.month);
    }
    public void OnDialogueEnded() {
        IsInDialogue = false;
        TryEvaluateSchedule();
    }
}
