using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public void StartDialogue(string[] lines)
    {
        foreach (string line in lines)
        {
            Debug.Log(line);
        }
    }
}
