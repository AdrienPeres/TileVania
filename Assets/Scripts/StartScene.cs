using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] GameObject skip;

    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = EventSystem.current;
    }
    void Start()
    {
        skip.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            OnClick();
        }
    }


    public void OnClick()
    {
        skip.SetActive(true);
        gameObject.SetActive(false);
        title.text = "";
        eventSystem.SetSelectedGameObject(GameObject.Find("Yes Button"));
    }
}
