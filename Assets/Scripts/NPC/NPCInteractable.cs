using UnityEngine;
using System.Collections;

public class NPCInteractable : MonoBehaviour {
    public NPCData npcData;

    public GameObject nameLabelObject;
    public TMPro.TextMeshPro nameText;
    public SpriteRenderer portraitRenderer;

    private bool isPlayerNearby = false;

    void Update() {
        if (!isPlayerNearby || DialogueManager.Instance.IsTalking() || DialogueManager.Instance.justFinishedDialogue) return;

        if (Input.GetKeyDown(KeyCode.E)) {
            isPlayerNearby = false; // prevent spamming during transition
            StartDialogueBasedOnQuestState();
            StartCoroutine(ReactivateTrigger());
        }
    }

    private void StartDialogueBasedOnQuestState() {
        var tracker = GameManager.Instance.playerQuestTracker;

        if (npcData == null) {
            Debug.LogError("❌ npcData is NULL on " + gameObject.name);
            return;
        }
        if (npcData.dialogueAsset == null) {
            Debug.LogError("❌ dialogueAsset is NULL on " + npcData.npcName);
            return;
        }

        Quest quest = tracker.activeQuests.Find(q => q.questID == npcData.dialogueAsset.linkedQuestID);

        if (quest == null) {
            // 👉 Initial interaction: play intro dialogue, then start quest
            DialogueManager.Instance.StartDialogue(npcData.dialogueAsset.initialEncounterLines, () => {
                var questData = QuestDatabase.Instance.GetQuestById(npcData.dialogueAsset.linkedQuestID);
                if (questData != null) {
                    var newQuest = new Quest {
                        questID = questData.questID,
                        title = questData.title,
                        description = questData.description,
                        isActive = true,
                        objectives = questData.objectives.ConvertAll(o => new QuestObjective {
                            type = o.type,
                            targetId = o.targetId,
                            requiredAmount = o.requiredAmount,
                            currentAmount = 0
                        })
                    };
                    tracker.StartQuest(newQuest);
                    tracker.EvaluateCollectItemObjectives(GameManager.Instance.playerInventory);
                    Debug.Log($"📜 Quest started: {newQuest.title}");
                }
            });
        }
        else if (!quest.IsCompleted && quest.isActive) {
            DialogueManager.Instance.StartDialogue(npcData.dialogueAsset.inProgressLines);
        }
        else if (quest.IsCompleted && quest.isActive) {
            DialogueManager.Instance.StartDialogue(npcData.dialogueAsset.questCompletedLines);
            quest.isActive = false;
        }
        else {
            DialogueManager.Instance.StartDialogue(npcData.dialogueAsset.postQuestLines);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }

    private void OnMouseEnter() {
        if (npcData != null && nameText != null) {
            nameText.text = npcData.npcName;
            nameLabelObject?.SetActive(true);
        }
    }

    private void OnMouseExit() {
        nameLabelObject?.SetActive(false);
    }

    void Start() {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize() {
        yield return new WaitForEndOfFrame();
        if (npcData != null && portraitRenderer != null) {
            portraitRenderer.sprite = npcData.portrait != null
                ? npcData.portrait
                : GameManager.Instance.defaultNPCPortrait;
        }
    }
    IEnumerator ReactivateTrigger() {
        yield return new WaitForSeconds(0.3f);
        isPlayerNearby = true;
    }
}