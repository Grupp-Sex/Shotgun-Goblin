using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevel : MonoBehaviour
{
    public NextLevelZone nextLevelZone;
    public string NextLevel;

    private void Awake()
    {
        if(nextLevelZone == null)
        {
            nextLevelZone = GetComponent<NextLevelZone>();
        }

        nextLevelZone.Event_GameEnd.Subscribe(Event_NextLevel);

    }

    protected void Event_NextLevel(object sender, object args)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(NextLevel);

    }

}
