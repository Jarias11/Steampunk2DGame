using UnityEditor.Build;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] public float sprintSpeed = 6f;


    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0;
    [SerializeField] private float dashCooldown = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 dashDirection;
    private float dashTimeRemaining;
    private float cooldownRemaining;
    private bool isDashing;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
    }

    void Update()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        isSprinting = !isDashing && Input.GetKey(KeyCode.LeftShift);



        //If not currently dashing, cooldown is 0 or less, space is pressed, and theres some movement, then start dash
        if (!isDashing && cooldownRemaining <= 0f && Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero)
        {
            StartDash();
        }
        //Timers
        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f || !Input.GetKey(KeyCode.Space)) EndDash();
        }
        else
        {
            cooldownRemaining -= Time.deltaTime;
        }



    }

    void FixedUpdate()
    {
        Vector2 displacement;
        // Update Animator bools
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsSprinting", isSprinting);

        // Determine speed
        float currentSpeed = isDashing ? dashSpeed :
                             isSprinting ? sprintSpeed :
                                           walkSpeed;
        // Calculate displacement
        displacement = isDashing
            ? dashDirection * currentSpeed * Time.fixedDeltaTime
            : movement * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + displacement);
    }
    private void StartDash()
    {
        isDashing = true;
        dashDirection = movement;          // dash *where the stick is pointing*
        dashTimeRemaining = dashDuration;
        cooldownRemaining = dashCooldown;

        // OPTIONAL: invincibility frames, trail FX, camera shake, SFX, etc.
        // GetComponent<Collider2D>().enabled = false;  // i‑frames example
    }
    private void EndDash()
    {
        isDashing = false;
        // GetComponent<Collider2D>().enabled = true;   // end of i‑frames
    }
}
