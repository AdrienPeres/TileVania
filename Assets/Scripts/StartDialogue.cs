using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueTrigger.TriggerDialogue();
    }

}
