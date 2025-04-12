using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private float inputDelay = 0.2f;
    private float timeSinceStart = 0f;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] currentLines;
    private int currentIndex = 0;
    private bool isTalking = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isTalking)
        {
            timeSinceStart += Time.deltaTime;

            if (timeSinceStart >= inputDelay && Input.GetKeyDown(KeyCode.E))
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;
        currentLines = lines;
        timeSinceStart = 0f;
        currentIndex = 0;
        isTalking = true;

        dialogueBox.SetActive(true);
        dialogueText.text = currentLines[currentIndex];
    }

    void DisplayNextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = currentLines[currentIndex];
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueBox.SetActive(false);
    }

    public bool IsTalking()
    {
        return isTalking;
    }
}