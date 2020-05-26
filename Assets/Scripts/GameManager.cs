using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager UI;

    public int[] levelDone = new int[7];

    private void Awake()
    {
        levelDone[0] = 1;
    }
    public void ToogleMenuPause()
    {
        if (UI.GetComponentInChildren<Canvas>().enabled)
        {
            UI.GetComponentInChildren<Canvas>().enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            UI.GetComponentInChildren<Canvas>().enabled = true;
            Time.timeScale = 0f;
        }

        if(GameObject.Find("Pause Canvas").GetComponent<Canvas>().enabled)
        {
            FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        }
        else
        {
            FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Play Button"));
        }
    }

    public UIManager GetUI()
    {
        return UI;
    }
}
