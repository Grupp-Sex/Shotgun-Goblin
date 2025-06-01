using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] bool isPlaying = false;
    [SerializeField] float volume = 1.0f;
    [SerializeField] float fade = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioManager audioManager = GameAudioManager.AudioManager;

            if (audioManager != null)
            {
                for(int i = 0; i < sounds.Count; i++)
                {
                    audioManager.PlayAmbient(sounds[i], volume, fade);
                }
                
            }
        }
    }
}
