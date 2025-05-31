using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameAudioManager
{
    public static AudioManager AudioManager { get; private set; }

    public static void SetAudioManager(AudioManager audioManager)
    {
        AudioManager = audioManager;
    }

}
