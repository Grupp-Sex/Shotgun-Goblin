/// MusicTrigger
/// 
/// This script is used to play background or ambient music when the player enters a trigger zone.
/// The music will start playing once, loop (if the AudioSource is set to loop), and the trigger 
/// will be disabled afterward to prevent replaying.
/// 
/// Setup Instructions:
/// - Attach this script to a GameObject with:
///     • An AudioSource component (with your desired audio clip).
///     • A Collider set to "Is Trigger".
/// - The player GameObject must have the tag "Player" and a Rigidbody component.
/// 
/// Note:
/// - You can use this script on multiple prefabs or GameObjects throughout your scenes.
///   To play different music in different locations, assign a different audio clip to each AudioSource
///   in those prefabs via the Unity Inspector.
/// 
/// Author: Mikael












using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MusicTrigger : MonoBehaviour
{


    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float volume = 0.4f;
    [SerializeField] private float fadeDuration = 1f;

    private bool isPlaying;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Getting tag");
            if (!isPlaying)
            {
                Debug.Log("Is now playing");
                isPlaying = true;
                if (GameAudioManager.AudioManager != null)
                {
                    Debug.Log("AudioManager was not null");

                    foreach (AudioClip clip in audioClips)
                    {
                        Debug.Log("Assign audio clip to playambience");
                        GameAudioManager.AudioManager.PlayAmbient(clip, volume, fadeDuration);
                    }
                }

            }

        }
    }
    //private AudioSource musicSource;
    //private bool hasPlayed;
    //private Collider triggerCollider;
    //void Awake()
    //{
    //    musicSource = GetComponent<AudioSource>();
    //    //SceneManager.activeSceneChanged += OnSceneChange;
    //    SceneManager.sceneUnloaded += OnSceneChange;
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //    triggerCollider = GetComponent<Collider>();

    //    // Reset state on scene load
    //    hasPlayed = false;
    //    if (triggerCollider != null)
    //    {
    //        triggerCollider.enabled = true;
    //    }
    //}

    //private void Start()
    //{
    //    ResetTrigger();
    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!hasPlayed && other.CompareTag("Player"))
    //    {
    //        musicSource.Play();
    //        hasPlayed = true;

    //        if (triggerCollider != null)
    //        {
    //            triggerCollider.enabled = false; //Prevent replay while still in scene
    //        }
    //    }
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    //Reset variables on every scene load so music can play again
    //    ResetTrigger();
    //}

    //private void OnSceneChange(Scene current/*, Scene next*/)
    //{
    //    if (musicSource != null && musicSource.isPlaying)
    //    {
    //        musicSource.Stop();
    //    }


    //    //hasPlayed = false;
    //    //if (triggerCollider != null)
    //    //{
    //    //    triggerCollider.enabled = true;
    //    //}
    //}

    //private void ResetTrigger()
    //{
    //    hasPlayed = false;
    //    if (triggerCollider != null)
    //    {
    //        triggerCollider.enabled = true;
    //    }
    //}

    //private void OnDestroy()
    //{

    //    //if ( musicSource != null && musicSource.isPlaying)
    //    //{
    //    //    musicSource.Stop();
    //    //}
    //    //SceneManager.activeSceneChanged -= OnSceneChange;

    //    SceneManager.sceneUnloaded -= OnSceneChange;
    //    SceneManager.sceneLoaded -= OnSceneLoaded;

    //}

}
