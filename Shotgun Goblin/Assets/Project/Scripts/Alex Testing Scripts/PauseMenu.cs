using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Coded by Alexandra Renner

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    //This bool will be needed as a reference in other
    //scripts so that no actions may occur when paused.
    public static bool isActive;

    void Start()
    {
        pauseMenu.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive) 
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isActive = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isActive = false;
    }
}
