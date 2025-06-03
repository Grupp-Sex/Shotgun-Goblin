using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnDash : GenralSoundPlayer
{
    public BaseDash Dash;

    // Start is called before the first frame update
    void Start()
    {
        if(Dash == null)  Dash = GetComponent<BaseDash>();

        
    }

    private void OnEnable()
    { 
        if(Dash != null) 
            Dash.Event_Dash.Subscribe(Event_Dash);
    }

    private void OnDisable()
    {
        if (Dash != null)
            Dash.Event_Dash.UnSubscribe(Event_Dash);
    }

    protected void Event_Dash(object sender, DashData dashData)
    {
        PlaySounds(sounds);
    }
}
