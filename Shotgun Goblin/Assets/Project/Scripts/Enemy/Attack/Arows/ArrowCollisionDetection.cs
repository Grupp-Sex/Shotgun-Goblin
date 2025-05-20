using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollisionDetection : MonoBehaviour, ICollision
{
    public EventPusher<Collision> Event_Collision { get; protected set; } = new EventPusher<Collision>();



    protected void OnCollisionEnter(Collision collision)
    {
        Event_Collision.Invoke(this,collision);
    }
}


public interface ICollision 
{
    EventPusher<Collision> Event_Collision { get; }
}
