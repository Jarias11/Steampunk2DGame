using UnityEditor.Build;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    [Header("Walk")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] public float sprintSpeed = 6f;


    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0;
    [SerializeField] private float dashCooldown = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private CameraZoomController zoomController;
    private Vector2 movement;
    private Vector2 lastMoveDir = Vector2.down;

    private Vector2 dashDirection;
    private float dashTimeRemaining;
    private float cooldownRemaining;
    private bool isDashing;
    private bool isSprinting;
    private bool isAttacking;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        zoomController = FindAnyObjectByType<CameraZoomController>();
    }

    void Start() {
    }

    void Update() {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Pick dominant axis for animation direction
        float moveX = movement.x;
        float moveY = movement.y;

        if (movement != Vector2.zero) {
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY)) {
                moveY = 0;
                moveX = Mathf.Sign(moveX);
            }
            else {
                moveX = 0;
                moveY = Mathf.Sign(moveY);
            }
        }
        if (movement != Vector2.zero && !isAttacking)
            lastMoveDir = movement;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);


        zoomController.IsSprinting = isSprinting;


        animator.SetFloat("Speed", movement.sqrMagnitude);
        isSprinting = !isDashing && Input.GetKey(KeyCode.LeftShift);




        //If not currently dashing, cooldown is 0 or less, space is pressed, and theres some movement, then start dash
        if (!isDashing && cooldownRemaining <= 0f && Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero) {
            StartDash();
        }
        //Timers
        if (isDashing) {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f || !Input.GetKey(KeyCode.Space)) EndDash();
        }
        else {
            cooldownRemaining -= Time.deltaTime;
        }
        if (!isAttacking && Input.GetKeyDown(KeyCode.Mouse0)) {
            GetComponent<PlayerAttack>().lastDirection = lastMoveDir;
            StartCoroutine(AttackRoutine());
        }



    }

    void FixedUpdate() {
        Vector2 displacement;
        // Update Animator bools
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsSprinting", isSprinting);

        // Determine speed
        float currentSpeed = isDashing ? dashSpeed :
                             isSprinting ? sprintSpeed :
                                           walkSpeed;
        // Calculate displacement
        displacement = isAttacking
            ? Vector2.zero // freeze movement during attack
            : (isDashing
            ? dashDirection * currentSpeed * Time.fixedDeltaTime
            : movement * currentSpeed * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + displacement);
    }
    private void StartDash() {
        isDashing = true;
        dashDirection = movement;          // dash *where the stick is pointing*
        dashTimeRemaining = dashDuration;
        cooldownRemaining = dashCooldown;

        // OPTIONAL: invincibility frames, trail FX, camera shake, SFX, etc.
        // GetComponent<Collider2D>().enabled = false;  // i‑frames example
    }
    private void EndDash() {
        isDashing = false;
        // GetComponent<Collider2D>().enabled = true;   // end of i‑frames
    }
    private IEnumerator AttackRoutine() {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        Vector2 attackDir = lastMoveDir.normalized;
        Transform spawn = GetComponent<PlayerAttack>().hitboxSpawn;
        spawn.localPosition = attackDir * 0.5f; // you can tweak 0.5f for reach

        // Optionally spawn hitbox early or with delay
        GetComponent<PlayerAttack>()?.PerformAttack();

        yield return new WaitForSeconds(0.4f); // Match animation length

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
