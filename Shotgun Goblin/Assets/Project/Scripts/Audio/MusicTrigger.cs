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



public class MusicTrigger : MonoBehaviour
{
    private AudioSource musicSource;
    private bool hasPlayed;
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            musicSource.Play();
            hasPlayed = true;
            GetComponent<Collider>().enabled = false;
        }
    }

}
