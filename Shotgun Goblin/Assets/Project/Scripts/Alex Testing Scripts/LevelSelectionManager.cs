using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public void LevelOne()
    {
        Game_Manager.ToggleMouse(false);
        UnloadCurrent();
        SceneManager.LoadScene("Level_1");
    }

    public void LevelTwo()
    {
        Game_Manager.ToggleMouse(false);
        UnloadCurrent();
        SceneManager.LoadScene("Level_2");
    }

    public void LevelThree()
    {
        Game_Manager.ToggleMouse(false);
        UnloadCurrent();
        SceneManager.LoadScene("Level_3");
    }

    public void DebugLevel()
    {


        Game_Manager.ToggleMouse(false);
        UnloadCurrent();
        SceneManager.LoadScene("TS1, Interactions and Movement");
    }

    public void TestLevelTwo()
    {
        SceneManager.LoadScene("TS2, Gameplay");
    }

    protected void UnloadCurrent()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
