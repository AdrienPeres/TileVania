using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    bool writingSentence = false;

    public bool dialogueRunning = false;

    public Queue<Sentence> sentences;

    public Sentence currentSentence;

    public Animator animator;

    Player player;

    void Start()
    {
        sentences = new Queue<Sentence>();
        player = FindObjectOfType<Player>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueRunning = true;
        dialogue.happenedOnce = true;
        animator.SetBool("IsOpen", true);
        player.stop = true;
        sentences.Clear();
        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);            
        }
        nameText.text = dialogue.name;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (writingSentence) { return; }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        currentSentence = sentences.Dequeue();
        if (currentSentence.isTrigger)
        {
            FindObjectOfType<Player>().SpeakingCameraTriggered(true);
        }
        else
        {
            FindObjectOfType<Player>().SpeakingCameraTriggered(false);
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence.content));
   
    }

    IEnumerator TypeSentence(string sentence)
    {
        writingSentence = true;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0);
        }
        writingSentence = false;
    }

    private void EndDialogue()
    {
        currentSentence.content = "";
        currentSentence.isTrigger = false;
        dialogueRunning = false;
        player.stop = false;
        animator.SetBool("IsOpen", false);
        FindObjectOfType<UIManager>().EndingDialogue();
    }
}
