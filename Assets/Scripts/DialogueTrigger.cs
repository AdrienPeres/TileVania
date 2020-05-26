using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool conversationRunning = false;

    

    public void TriggerDialogue()
    {
        if (FindObjectOfType<GameManager>().levelDone[SceneManager.GetActiveScene().buildIndex] != 0) { dialogue.happenedOnce = true; }
        if (conversationRunning) { return; }
        if (dialogue.happenedOnce) { return; }
        else
        {
            conversationRunning = true;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
