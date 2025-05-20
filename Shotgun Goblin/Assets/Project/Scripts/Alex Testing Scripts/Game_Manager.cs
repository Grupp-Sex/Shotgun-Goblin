using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public GameObject gameOverUI;
    //public GameObject optionsUI;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("TS2, Gameplay"); //Game file and it's components must be added here
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    /*Reloads the active scene, as long as it's in the build-index.
     Scenes can be added to the build-index.*/
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Switches scenes within the build-index
    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu"); 
    }

    public void Levels()
    {
        SceneManager.LoadScene("Level_Selection");
    }
    public void Options()
    {
        SceneManager.LoadScene("Options_Menu");
    }

    public void QuitProgram()
    {
        Application.Quit();
    }
}
