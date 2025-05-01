using System.Numerics;
using Unity.Cinemachine;
using UnityEngine;

public class NPCWalking : MonoBehaviour {
    public float moveSpeed = 1f;
    public float walkDistance = 5;
    public float waitTime = 1f;

    private UnityEngine.Vector3[] directions = new UnityEngine.Vector3[]{
        UnityEngine.Vector3.down,
        UnityEngine.Vector3.left,
        UnityEngine.Vector3.up,
        UnityEngine.Vector3.right

    };

    private int currentDirection = 0;
    private bool goingOut = true;
    private UnityEngine.Vector3 walkStartPos;
    private UnityEngine.Vector3 targetPosition;

    private bool isWalking = true;
    private float waitTimer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkStartPos = transform.position;
        SetTarget();
    }
    void Update() {
        if (isWalking) {
            // ✅ Only move and animate when walking
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            AnimateDirection(targetPosition - transform.position);

            if (UnityEngine.Vector3.Distance(transform.position, targetPosition) < 0.01f) {
                isWalking = false;
                waitTimer = waitTime;
                ResetAnimationBools();
            }
        }
        else {
            // ✅ Wait between moves
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) {
                isWalking = true;

                if (goingOut) {
                    // Come back to start
                    goingOut = false;
                    targetPosition = walkStartPos;
                }
                else {
                    // Start new direction
                    currentDirection = (currentDirection + 1) % directions.Length;
                    walkStartPos = transform.position; // update start
                    goingOut = true;
                    SetTarget();
                }
            }
        }
    }
    void SetTarget() {
        UnityEngine.Vector3 direction = directions[currentDirection];
        targetPosition = walkStartPos + direction * walkDistance;
    }

    void AnimateDirection(UnityEngine.Vector3 moveVector) {
        ResetAnimationBools();

        UnityEngine.Vector3 dir = moveVector.normalized;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
            // Horizontal
            if (dir.x > 0)
                animator.SetBool("isWalkingRight", true);
            else
                animator.SetBool("isWalkingLeft", true);

        }
        else {
            // Vertical
            if (dir.y > 0)
                animator.SetBool("isWalkingUp", true);
            else
                animator.SetBool("isWalkingDown", true);
        }
    }

    void ResetAnimationBools() {
        animator.SetBool("isWalkingDown", false);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingUp", false);
        animator.SetBool("isWalkingRight", false);
    }
}



