using UnityEngine;
using System.Collections; // Required for IEnumerator

public class NPCInteractable : MonoBehaviour
{
    public NPCData npcData;
    public GameObject nameLabelObject;
    public TMPro.TextMeshPro nameText;
    public SpriteRenderer portraitRenderer;
    private bool isPlayerNearby = false;

void Update()
{
    if (isPlayerNearby && !DialogueManager.Instance.IsTalking() && Input.GetKeyDown(KeyCode.E))
    {
        DialogueManager.Instance.StartDialogue(npcData.dialogueLines);
    }
}

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isPlayerNearby = true;
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isPlayerNearby = false;
    }
}

    private void OnMouseEnter()
    {
        if (npcData != null && nameText != null)
        {
            nameText.text = npcData.npcName;
            nameLabelObject?.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        nameLabelObject?.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame(); // Ensure GameManager is ready

        if (npcData != null && portraitRenderer != null)
        {
            portraitRenderer.sprite = npcData.portrait != null
                ? npcData.portrait
                : GameManager.Instance.defaultNPCPortrait;
        }
    }
}
