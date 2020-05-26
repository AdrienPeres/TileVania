using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionChoice : MonoBehaviour
{
    [SerializeField] GameObject pauseButton;

    private GameObject dialogueButton;
    private EventSystem eventSystem;
    private DialogueManager dialogueManager;
    private Canvas pauseCanvas;

    private GameObject playButton;

    private void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        pauseCanvas = GameObject.Find("Pause Canvas").GetComponent<Canvas>();
        playButton = GameObject.Find("Play Button");
        if (dialogueManager != null)
        {
            dialogueButton = GameObject.Find("Continue");
        }
    }

    void Update()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueButton = GameObject.Find("Continue");

        if (dialogueManager != null)
        {
            WithDialogue();
        }
        else
        {
            WithoutDialogue();
        }

        if(eventSystem.currentSelectedGameObject == null)
        {
            if(playButton == null)
            {
                eventSystem.SetSelectedGameObject(GameObject.Find("Yes Button"));
            }
            else
            {
                eventSystem.SetSelectedGameObject(playButton);
            }   
        }

        if (FindObjectOfType<UIManager>().NextSceneLoaded())
        {
            Debug.Log("Next Scene Loaded");
            dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueButton = GameObject.Find("Continue");
            eventSystem = FindObjectOfType<EventSystem>();
            playButton = GameObject.Find("Play Button");
        }

    }

    private void WithoutDialogue()
    {
        
        if (pauseCanvas.enabled)
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(pauseButton);
            }
        }
        else
        {
            if (eventSystem.currentSelectedGameObject == pauseButton)
            {
                eventSystem.SetSelectedGameObject(null);
            }
        }
    }

    private void WithDialogue()
    {

        if (pauseCanvas.enabled)
        {
            dialogueButton.GetComponent<Button>().interactable = false;
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(pauseButton);
            }
        }
        else if (dialogueManager.dialogueRunning)
        {
            dialogueButton.GetComponent<Button>().interactable = true;
            eventSystem.SetSelectedGameObject(dialogueButton);
        }
        else
        {
            if (eventSystem.currentSelectedGameObject == pauseButton)
            {
                eventSystem.SetSelectedGameObject(null);
            }
            dialogueButton.GetComponent<Button>().interactable = false;
        }
    }
}
