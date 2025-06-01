using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ansgar
public class VictorySound : GenralSoundPlayer
{
    public OnVictory onVictory;

    private void Awake()
    {
        if (onVictory == null)
        {
            onVictory = GetComponent<OnVictory>();
        }

        onVictory.Event_Victory.Subscribe(Event_Victory);
    }

    protected void Event_Victory(object sender, object args)
    {
        PlaySounds(sounds);
    }
}
