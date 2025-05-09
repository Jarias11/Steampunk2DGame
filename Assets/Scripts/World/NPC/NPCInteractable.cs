using UnityEngine;
using System.Collections;

public class NPCInteractable : MonoBehaviour {
    public NPCProfile npcProfile;

    public GameObject nameLabelObject;
    public TMPro.TextMeshPro nameText;
    public SpriteRenderer portraitRenderer;
    private NPCController controller;
    private bool isPlayerNearby = false;

    void Update() {
        if (!isPlayerNearby || DialogueManager.Instance.IsTalking() || DialogueManager.Instance.justFinishedDialogue) return;
        if (Input.GetKeyDown(KeyCode.E)) {

            StartDialogueBasedOnQuestState();
            StartCoroutine(ReactivateTrigger());
        }
    }

    private void StartDialogueBasedOnQuestState() {
        var tracker = GameManager.Instance.playerQuestTracker;

        if (npcProfile == null) {
            Debug.LogError("❌ npcData is NULL on " + gameObject.name);
            return;
        }
        if (controller != null) {
            controller.SwitchState(new NPCTalkState());
        }

        Quest quest = tracker.activeQuests.Find(q => q.questID == npcProfile.linkedQuestID);

        if (quest == null) {
            // 👉 Initial interaction: play intro dialogue, then start quest
            DialogueManager.Instance.StartDialogue(npcProfile.initialEncounterLines, () => {
                var questData = QuestDatabase.Instance.GetQuestById(npcProfile.linkedQuestID);
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
                HandleDialogueEnd();
            }, this);
        }
        else if (!quest.IsCompleted && quest.isActive) {
            DialogueManager.Instance.StartDialogue(npcProfile.inProgressLines, HandleDialogueEnd, this);
            HandleDialogueEnd();
        }
        else if (quest.IsCompleted && quest.isActive) {
            DialogueManager.Instance.StartDialogue(npcProfile.questCompletedLines, () => {
                HandleDialogueEnd();
                quest.isActive = false;
            }, this);
        }
        else {
            DialogueManager.Instance.StartDialogue(npcProfile.questCompletedLines, () => {
                HandleDialogueEnd();
                quest.isActive = false;
            }, this);
        }
    }

    void Start() {
        StartCoroutine(Initialize());
        controller = GetComponent<NPCController>();
    }

    IEnumerator Initialize() {
        yield return new WaitForEndOfFrame();
        if (npcProfile != null && portraitRenderer != null) {
            portraitRenderer.sprite = npcProfile.portrait != null
                ? npcProfile.portrait
                : GameManager.Instance.defaultNPCPortrait;
        }
    }
    IEnumerator ReactivateTrigger() {
        yield return new WaitForSeconds(0.3f);
        isPlayerNearby = true;
    }
    private void HandleDialogueEnd() {
        controller?.OnDialogueEnded();
    }

    public bool IsPlayerNearby() => isPlayerNearby;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }

    private void OnMouseEnter() {
        if (npcProfile != null && nameText != null) {
            nameText.text = npcProfile.npcName;
            nameLabelObject?.SetActive(true);
        }
    }

    private void OnMouseExit() {
        nameLabelObject?.SetActive(false);
    }
    private void OnDestroy() {

    }
}