using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Coded by Alexandra Renner


public class LevelSelectionManager : MonoBehaviour
{
    public void LevelOne()
    {
        Game_Manager.ToggleMouse(false);
        SceneManager.LoadScene("Level_1");
    }

    public void LevelTwo()
    {
        Game_Manager.ToggleMouse(false);
        SceneManager.LoadScene("Level_2");
    }

    public void LevelThree()
    {
        Game_Manager.ToggleMouse(false);
        SceneManager.LoadScene("Level_3");
    }

    public void TestLevelTwo()
    {
        SceneManager.LoadScene("TS2, Gameplay");
    }

    
}
