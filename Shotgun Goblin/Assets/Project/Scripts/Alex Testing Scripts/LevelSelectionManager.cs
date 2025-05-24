using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public void LevelOne()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void TestLevelOne()
    {
        SceneManager.LoadScene("TS1, Interactions and Movement");
    }

    public void TestLevelTwo()
    {
        SceneManager.LoadScene("TS2, Gameplay");
    }
}
