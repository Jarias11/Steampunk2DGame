using UnityEngine;
using UnityEngine.AI;

public class EnemySM : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public NavMeshAgent Agent { get; private set; }

    [HideInInspector] public Transform Player;
    private EnemyBase currentState;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.Warp(transform.position);
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
}
