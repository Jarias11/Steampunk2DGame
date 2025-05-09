using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance;
    private float inputDelay = 0.2f;
    private float timeSinceStart = 0f;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] currentLines;
    private int currentIndex = 0;
    private bool isTalking = false;
    public bool justFinishedDialogue = false;
    public System.Action onDialogueComplete;
    private NPCInteractable currentNPCInteractable;

    void Awake() {
        Instance = this;
    }

    void Update() {
        if (isTalking && currentNPCInteractable != null && !currentNPCInteractable.IsPlayerNearby()) {
            Debug.Log("🛑 Player walked away. Ending dialogue.");
            EndDialogue();
        }
        if (isTalking) {
            timeSinceStart += Time.deltaTime;

            if (timeSinceStart >= inputDelay && Input.GetKeyDown(KeyCode.E)) {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(string[] lines) {
        StartDialogue(new System.Collections.Generic.List<string>(lines));
    }

    public void StartDialogue(System.Collections.Generic.List<string> lines, System.Action onComplete = null, NPCInteractable npc = null) {
        currentNPCInteractable = npc;
        if (lines == null || lines.Count == 0) return;
        currentLines = lines.ToArray();
        timeSinceStart = 0f;
        currentIndex = 0;
        isTalking = true;
        onDialogueComplete = onComplete;

        dialogueBox.SetActive(true);
        dialogueText.text = currentLines[currentIndex];
    }

    void DisplayNextLine() {

        currentIndex++;
        if (currentIndex >= currentLines.Length) {
            EndDialogue();
        }
        else {
            dialogueText.text = currentLines[currentIndex];
        }

    }

    void EndDialogue() {
        isTalking = false;
        dialogueBox.SetActive(false);
        onDialogueComplete?.Invoke();
        onDialogueComplete = null;

        // debounce input
        justFinishedDialogue = true;
        Invoke(nameof(ClearDialogueCooldown), 0.3f);
    }

    public bool IsTalking() {
        return isTalking;
    }
    void ClearDialogueCooldown() {
        justFinishedDialogue = false;
    }
}