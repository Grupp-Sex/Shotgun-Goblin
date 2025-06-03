using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTelekenesisSound : GenralSoundPlayer, IOnTelekenesis, IOnTelekenesisThrow
{


    [SerializeField] bool OnStart;
    [SerializeField] bool OnEnd;
    [SerializeField] bool OnThrow;

    public void OnTelekenesisStart()
    {
        if (OnStart)
        {
            PlaySounds(sounds);
        }
    }

    public void OnTelekenesisEnd()
    {
        if (OnEnd)
        {
            PlaySounds(sounds);
        }
    }

    public void OnTelekenesisThrow()
    {
        if (OnThrow)
        {
            PlaySounds(sounds);
        }
    }

    
}
