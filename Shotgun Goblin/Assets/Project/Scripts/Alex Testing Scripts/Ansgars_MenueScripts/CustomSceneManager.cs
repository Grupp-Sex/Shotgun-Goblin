using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CustomSceneManager
{

    // alternative to SceneManager.LoadScene(sceneName)
    // used to revisit loaded scenes without reloading them. For scenes sutch as settigns where states need to be saved

    // DOES NOT WORK
    public static Scene GoToScene(string sceneName)
    {
        try
        {
            Scene targetScene = SceneManager.GetSceneByName(sceneName);

            if (!SceneManager.SetActiveScene(targetScene))
            {
                SceneManager.LoadScene(sceneName);
            }

        }
        catch
        {
            SceneManager.LoadScene(sceneName);
            
        }

        return SceneManager.GetSceneByName(sceneName); 
    }
}
