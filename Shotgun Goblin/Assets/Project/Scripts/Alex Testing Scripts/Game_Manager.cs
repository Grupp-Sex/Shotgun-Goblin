using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenu;
    public bool lockMouse;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // checks if either of the ui canvases exist and is inactive
    protected bool CheckIfUiExist()
    {
        lockMouse = (gameOverUI != null && gameOverUI.activeInHierarchy) || (pauseMenu != null && pauseMenu.activeInHierarchy);

         return lockMouse;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfUiExist();


        ToggleMouse(lockMouse);


    }

    public void PlayGame()
    {
        ToggleMouse(false);

        SceneManager.LoadScene("Level_1"); //Game file and it's components must be added here
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
        ToggleMouse(true);
        SceneManager.LoadScene("Main_Menu"); 
    }

    public void Levels()
    {
        ToggleMouse(true);

        SceneManager.LoadScene("Level_Selection");
    }
    public void Tutorial()
    {
        ToggleMouse(false);

        SceneManager.LoadScene("Tutorial01");
    }

    public void Options()
    {
        ToggleMouse(true);

        SceneManager.LoadScene("Options_Menu");
    }

    

    public static void ToggleMouse(bool active)
    {

        // original code by Alex, move here by ansgar
        if (active)
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

    


    public void QuitProgram()
    {
        Application.Quit();
    }
}
