using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mecanisms : MonoBehaviour
{
    [SerializeField] DialogueManager DManager;
    [SerializeField] GameObject levelExit;

    [SerializeField] BoxCollider2D messageCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TilemapRenderer>().enabled = false;
        GetComponent<TilemapCollider2D>().enabled = false;
        levelExit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(DManager.currentSentence.isTrigger)
        {
            GetComponent<TilemapRenderer>().enabled = true;
            GetComponent<TilemapCollider2D>().enabled = true;
            levelExit.SetActive(true);
            messageCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
