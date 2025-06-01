/// Manages persistent main menu music across menu-related scenes.
/// Destroys itself automatically when transitioning to non-menu scenes (e.g., gameplay).
//Author: Mikael
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMusicManager : MonoBehaviour
{


    private static MainMusicManager instance;

    private AudioSource musicSource;

    [SerializeField] private string[] MainMenuScenes = { "Main_Menu", "Options_Menu", "Level_Selection" }; //Assign scenes included in main menu
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); //preventing duplicate
        }
        
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isAllowed = false;

        foreach (string s in MainMenuScenes)
        {
            if (scene.name == s)
            {
                isAllowed = true;
                break;
            }
        }

        if (!isAllowed)
        {
            Destroy(gameObject); //Scene not in allowed list
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
