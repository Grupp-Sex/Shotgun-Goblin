using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnDestroy : MonoBehaviour
{
    public EventPusher<object> Event_OnDisable = new EventPusher<object>();


    private void OnDisable()
    {
        Event_OnDisable.Invoke(this,null);
    }

    private void OnDestroy()
    {
        Event_OnDisable.UnSubscribeAll();
    }

}
