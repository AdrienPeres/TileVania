using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Message : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    DialogueManager dialogueManager;
    [SerializeField] TipCollider tipCollider;

    public bool otherTrigger;
    public bool tutoMessage;
    public bool activeState = false;

    private bool monologueRunning = false;
    private bool onCollision = false;
    private bool exitCollision = false;
    private BoxCollider2D tipBoxCollider;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        tipBoxCollider = tipCollider.GetBoxCollider2D();
        ToggleTip();
        if(tutoMessage)
        {
            tipBoxCollider.enabled = false;
        }
    }

    private void ToggleTip()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(activeState);
        }
        GetComponent<Text>().enabled = activeState;
    }

    void Update()
    {
        onCollision = tipCollider.GetOnCollision();
        exitCollision = tipCollider.GetOnExitCollision();

        if (!otherTrigger)
        {
            if (onCollision)
            {
                activeState = true;
                ToggleTip();
            }
            
            if(exitCollision)
            {
                activeState = false;
                ToggleTip();

                if(!tutoMessage)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (dialogueManager != null)
            {
                if (dialogueManager.dialogueRunning)
                {
                    monologueRunning = true;
                    tipBoxCollider.enabled = true;
                }
                if (!dialogueManager.dialogueRunning && monologueRunning)
                {
                    monologueRunning = false;
                    activeState = true;
                    otherTrigger = false;
                    ToggleTip();
                }
                if(exitCollision)
                {
                    Debug.Log("Exit Tip after dialogue");
                    
                    activeState = false;
                    ToggleTip();
                    if(!tutoMessage)
                    {
                        Destroy(gameObject);
                    }
                    
                }
            }
        }      
    }
}
