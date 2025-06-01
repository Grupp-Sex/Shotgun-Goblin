using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] List<AudioClip> Sounds;


    private void Start()
    {
        PlaySounds();
    }

    protected void PlaySounds()
    {
        if(GameAudioManager.AudioManager != null)
        {
            for(int i = 0; i < Sounds.Count; i++)
            {
                GameAudioManager.AudioManager.PlayPooledSound(Sounds[i],true, false);

            }
        }
    }
}
