using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class DestructableGrass : MonoBehaviour, IDestructibleWorldObject {
    [SerializeField] private string prefabID = "tall_grass";
    private bool isDestroyed = false;

    private Animator animator;
    private Transform player;
    private bool playerInRange = false;

    private void Awake() {
        animator = GetComponent<Animator>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update() {
        if (playerInRange && player != null) {
            float speed = player.GetComponent<Animator>()?.GetFloat("Speed") ?? 0f;
            bool isMoving = speed > 0.01f;
            animator.SetBool("IsShaking", isMoving);
            Debug.Log($"Animator IsShaking: {animator.GetBool("IsShaking")}");
        }
        else {
            animator.SetBool("IsShaking", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player = other.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            player = null;
            animator.SetBool("IsShaking", false);
        }
    }

    // --- Save Interface ---

    public string GetID() => prefabID;
    public Vector2 GetPosition() => transform.position;
    public bool IsDestroyed() => isDestroyed;

    public WorldObjectData GetSaveData() {
        return new WorldObjectData {
            id = prefabID,
            prefabID = prefabID,
            position = transform.position,
            isDestroyed = isDestroyed,
            growthStage = 0
        };
    }

    public void LoadFromData(WorldObjectData data) {
        transform.position = data.position;
        isDestroyed = data.isDestroyed;
    }
}
