using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public void StartFirstLevel()
    {
        SceneManager.LoadScene(2);
    }
    
    public void PlayAgain()
    {
        if(FindObjectOfType<GameSession>())
        {
            FindObjectOfType<GameSession>().ResetGameSession();
        }
        SceneManager.LoadScene(2);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartFromMenuPause()
    {
        if (FindObjectOfType<GameSession>())
        {
            FindObjectOfType<GameSession>().ResetGameSession();
        }
        SceneManager.LoadScene(2);
        if (FindObjectOfType<GameManager>())
        {
            FindObjectOfType<GameManager>().ToogleMenuPause();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainTitle()
    {
        if (FindObjectOfType<GameSession>())
        {
            FindObjectOfType<GameSession>().ResetGameSession();
        }
        SceneManager.LoadScene(0);
    }

}
