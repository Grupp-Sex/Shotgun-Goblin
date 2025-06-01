using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Author Mikael
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
