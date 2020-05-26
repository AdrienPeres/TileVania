using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject firstButton;
    [SerializeField] GameObject[] pauseButtons;

    private Button startButton;
    private Button quitButton;

    private MusicManager musicManager;
    private GameObject secondButton;

    private DialogueManager dialogueManager;
    private Slider musicSlider;
    private Toggle musicToggle;
    private Toggle soundsToggle;
    private Canvas pauseCanvas;
    private EventSystem events;

    Player player;
    CoinPickup[] coins;

    int initialSceneIndex;
    int currentSceneIndex;

    private void Start()
    {
        musicSlider = GameObject.Find("Slider Volume").GetComponent<Slider>();
        musicToggle = GameObject.Find("Music Toggle").GetComponent<Toggle>();
        pauseCanvas = GameObject.Find("Pause Canvas").GetComponent<Canvas>();
        soundsToggle = GameObject.Find("Effects Toggle").GetComponent<Toggle>();

        musicManager = FindObjectOfType<MusicManager>();
        events = EventSystem.current;

        initialSceneIndex = 0;
    }

    private void Update()
    {

        ScanForKeyStroke();
        MusicSliderUpdate(musicSlider.value);
        Interactables();
        if (dialogueManager != null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
            //Debug.Log(dialogueManager.dialogueRunning);
            IfDialogueRunning();
        }
        if (events.currentSelectedGameObject == null && !pauseCanvas.enabled && secondButton != null)
        {
            events.SetSelectedGameObject(secondButton);
        }

        if(NextSceneLoaded())
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
            musicManager = FindObjectOfType<MusicManager>();
            secondButton = GameObject.Find("Play Button");
            if(secondButton != null)
            {
                startButton = secondButton.GetComponent<Button>();
            }
            if(GameObject.Find("Other Button") != null)
            {
               quitButton = GameObject.Find("Other Button").GetComponent<Button>();
            }            
        }

    }

    public bool NextSceneLoaded()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != initialSceneIndex)
        {
            initialSceneIndex = currentSceneIndex;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Interactables()
    {
        if(currentSceneIndex == 0 || currentSceneIndex == 1)
        {
            pauseButtons[0].SetActive(pauseCanvas.enabled);
            pauseButtons[1].SetActive(false);
            pauseButtons[2].SetActive(false);
        }
        else
        {
            foreach (GameObject pauseButton in pauseButtons)
            {
                pauseButton.SetActive(pauseCanvas.enabled);
            }
        }
        
        if(pauseCanvas.enabled)
        {
            if (events.currentSelectedGameObject == null)
            {
                events.SetSelectedGameObject(firstButton);
            }
            if (startButton != null) { startButton.interactable = false; }
            if(quitButton != null) { quitButton.interactable = false; }
            musicSlider.interactable = true;
            musicToggle.interactable = true;
            soundsToggle.interactable = true;
        }
        else
        {
            musicSlider.interactable = false;
            musicToggle.interactable = false;
            soundsToggle.interactable = false;
            if (startButton != null) { startButton.interactable = true; }
            if (quitButton != null) { quitButton.interactable = true; }
        }
        
    }

    private void IfDialogueRunning()
    {
        if(startButton != null && quitButton != null)
        {
            startButton.interactable = !dialogueManager.dialogueRunning;
            quitButton.interactable = !dialogueManager.dialogueRunning;
        }
        
    }

    public void EndingDialogue()
    {
        events.SetSelectedGameObject(secondButton);
    }

    private void ScanForKeyStroke()
    {
        if(Input.GetKeyDown("escape") || Input.GetButtonDown("Pause"))
        { 
            gameManager.ToogleMenuPause();
        }
    }

    public void MusicSliderUpdate(float val)
    {
        musicManager.SetVolume(val);
    }

    public void MusicToggle()
    {
        musicManager.GetComponent<AudioSource>().mute = !musicToggle.isOn;
    }

    public void SoundsToggle()
    {
        player = FindObjectOfType<Player>();
        coins = FindObjectsOfType<CoinPickup>();
        LevelExit levelExit = FindObjectOfType<LevelExit>();
        foreach (CoinPickup coin in coins)
        {
            coin.GetComponent<AudioSource>().mute = !soundsToggle.isOn;
        }
        player.GetComponent<AudioSource>().mute = !soundsToggle.isOn;
        levelExit.GetComponent<AudioSource>().mute = !soundsToggle.isOn;
    }
}
