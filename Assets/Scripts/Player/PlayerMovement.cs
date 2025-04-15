using UnityEditor.Build;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] public float moveSpeed = 5f;


    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0;
    [SerializeField] private float dashCooldown = 0;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 dashDirection;
    private float dashTimeRemaining;
    private float cooldownRemaining;
    private bool isDashing;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start()
    {
    }

    void Update()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        //If not currently dashing, cooldown is 0 or less, space is pressed, and theres some movement, then start dash
        if (!isDashing && cooldownRemaining <= 0f && Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero)
        {
            StartDash();
        }
        //Timers
        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f) EndDash();
        }
        else
        {
            cooldownRemaining -= Time.deltaTime;
        }
        if (isDashing && !Input.GetKey(KeyCode.Space))
        {
            EndDash();        // terminate immediately
        }



    }

    void FixedUpdate()
    {
        Vector2 displacement;

        if (isDashing)
        {
            displacement = dashDirection * dashSpeed * Time.fixedDeltaTime;
        }
        else
        {
            displacement = movement * moveSpeed * Time.fixedDeltaTime;
        }

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
