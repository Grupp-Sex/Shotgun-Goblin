using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnRagdoll : MonoBehaviour
{
    public RagdollManager ragdoll;
    public bool EnableOnRagdoll = true;
    public bool DoToggle = true;


    private void Awake()
    {
        ragdoll.Event_RagdollStart.Subscribe(Event_RagdollStart);
        ragdoll.Event_RagdollEnd.Subscribe(Event_RagdollEnd);

        ToggleObject(!EnableOnRagdoll);
    }

    private void OnDestroy()
    {
        ragdoll.Event_RagdollStart.UnSubscribe(Event_RagdollStart);
        ragdoll.Event_RagdollEnd.UnSubscribe(Event_RagdollEnd);
    }

    protected void Event_RagdollStart(object sender, object args)
    {
        ToggleObject(EnableOnRagdoll);
    }

    protected void Event_RagdollEnd(object sender, object args)
    {
        ToggleObject(!EnableOnRagdoll);
    }

    protected void ToggleObject(bool enable)
    {
        if (DoToggle)
        {
            gameObject.SetActive(enable);
        }
    }
    
}
