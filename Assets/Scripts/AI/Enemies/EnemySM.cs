using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyDataProvider))]
public class EnemySM : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 2f;

    [Header("Vision")]
    public LayerMask visionMask;
    public UnityEngine.Vector2 eyeOffset = new Vector2(0f, 0.3f);


    [Header("Combat")]
    public float windUpTime = 0.30f;
    public float hitboxTime = 1.00f;
    public float cooldownTime = 0.70f;
    public GameObject hitboxPrefab;

    //Runtime variables

    public NavMeshAgent Agent { get; private set; }
    [HideInInspector] public Transform Player;
    public SpriteRenderer spriteRenderer;

    public UnityEngine.Vector3 spawnPos { get; private set; }
    public UnityEngine.Vector3 lastSeenPlayerPos { get; private set; }
    public EnemyDataProvider Data { get; private set; }

    private EnemyBase currentState;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Agent = GetComponent<NavMeshAgent>();
        spawnPos = transform.position;
        Data = GetComponent<EnemyDataProvider>();
        if (Data == null)
            Debug.LogError("EnemyDataProvider not found on enemy!");

    }
    private void Start()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player = playerObj.transform;
            currentState = new EnemyIdle();  // ← Only switch if player exists
            currentState.EnterState(this);
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure they are tagged 'Player'.");
        }
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }
    public void SwitchState(EnemyBase newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void flip(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        if (direction.x > 0.01f)
            spriteRenderer.flipX = true; // Facing right
        else if (direction.x < -0.01f)
            spriteRenderer.flipX = false;  // Facing left
    }

    public bool canSeePlayer()
    {
        if (Player == null) return false;
        Vector2 toPlayer = Player.position - transform.position;
        float distSq = toPlayer.sqrMagnitude;
        float maxSq = chaseRange * chaseRange;

        if (distSq > maxSq) return false;


        Vector2 origin = (Vector2)transform.position + eyeOffset;
        Vector2 dir = toPlayer.normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            origin, dir, chaseRange, visionMask
        );
        return hit && hit.collider.CompareTag("Player");
    }

    public void LostPlayer(Vector3 lastPos)
    {
        lastSeenPlayerPos = lastPos;
        SwitchState(new EnemySearch(lastSeenPlayerPos));

    }

}
