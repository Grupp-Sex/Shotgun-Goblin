using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventPusher<T>
{
    public event Action<object, T> EVENT;

    private EventPusher<AdvancedEventArgs> event_Invokation_Logger;

    public EventPusher() { }

    public EventPusher(EventPusher<AdvancedEventArgs> event_Invokation_Logger)
    {
        this.event_Invokation_Logger = event_Invokation_Logger;
    }

    public bool IsSubscriber(Action<object, T> _handeler)
    {
        bool subscribed = false;

        if (EVENT != null)
        {
            subscribed = EVENT.GetInvocationList().Contains(_handeler);
        }


        return subscribed;
    }

    public void Subscribe(Action<object, T> _handeler)
    {
        if (!IsSubscriber(_handeler))
        {
            EVENT += _handeler;

        }
    }

    public void UnSubscribe(Action<object, T> _handeler)
    {
        if (IsSubscriber(_handeler))
        {
            EVENT -= _handeler;
        }
    }

    public void UnSubscribeAll()
    {
        if (EVENT != null)
        {
            Action<object, T>[] subscribers = EVENT.GetInvocationList() as Action<object, T>[];

            for (int i = 0; i < subscribers?.Count(); i++)
            {
                EVENT -= subscribers[i];
            }
        }

    }

   


    public void Invoke(object sender, T args)
    {
        Invoke(sender, args, "");

    }

    public void Invoke(object sender, T args, string eventLog)
    {
        event_Invokation_Logger?.Invoke(this, new AdvancedEventArgs { eventPusher = this, sender = sender, args = args, EventLog = eventLog });
        EVENT?.Invoke(sender, args);

    }

}

public struct AdvancedEventArgs
{
    public object eventPusher;
    public object sender;
    public object args;

    public string EventLog;

}
