using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float LevelExitSlowMoFactor = 0.2f;
    [SerializeField] AudioClip portalTransfer;
    bool isActivated = false;

    Animator portalAnimator;

    private void Start()
    {
        portalAnimator = GetComponent<Animator>();
        if(FindObjectsOfType<Switch>().Length > 0)
        {
            isActivated = false;
        }
        else
        {
            isActivated = true;
        }
    }

    private void Update()
    {
        PortalActivation();
        if(isActivated)
        {
            portalAnimator.SetBool("Activating", true);
        }
        else
        {
            portalAnimator.SetBool("Activating", false);
        }
    }

    private void PortalActivation()
    {
        var count = 0;
        foreach(Switch lever in FindObjectsOfType<Switch>())
        {
            if(lever.IsActivated())
            {
                count++;
            }
        }

        if(count == FindObjectsOfType<Switch>().Length)
        {
            isActivated = true;
        }
        else
        {
            isActivated = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isActivated) { return; }
        AudioSource.PlayClipAtPoint(portalTransfer, Camera.main.transform.position);
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        Time.timeScale = LevelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<GameManager>().levelDone[currentSceneIndex] = 1;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
