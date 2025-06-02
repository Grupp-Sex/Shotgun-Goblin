using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : GenralSoundPlayer
{
   


    private void OnEnable()
    {
        PlaySounds(sounds);
    }

    
}
