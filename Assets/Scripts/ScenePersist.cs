using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private int sceneIndex;
    private void Awake()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        ScenePersist[] persists = FindObjectsOfType<ScenePersist>();
        foreach (var persist in persists)
        {
            if (persist != this)
            {
                if (persist.sceneIndex == currentSceneIndex)
                {
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    Destroy(persist.gameObject);
                }
            }
        }
        sceneIndex = currentSceneIndex;
        DontDestroyOnLoad(gameObject);
    }

    /*int startingSceneIndex;

    private void Awake()
    {
        int numPersists = FindObjectsOfType<ScenePersist>().Length;
        if (numPersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        Debug.Log("starting scene index " + startingSceneIndex);   
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("current scene index " + currentSceneIndex);
        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }
    }
*/
}
