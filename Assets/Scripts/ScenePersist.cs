using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private int sceneIndex;
    int currentSceneIndex;
    private void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
    void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 7)
        {
            Destroy(gameObject);
        }
    }
}
