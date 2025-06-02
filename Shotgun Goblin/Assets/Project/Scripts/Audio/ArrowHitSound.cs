using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitSound : GenralSoundPlayer
{
    public ArrowCollisionDetection collisionDetection;

    // Start is called before the first frame update
    void Start()
    {
        if( collisionDetection == null)
        {
            collisionDetection = GetComponent<ArrowCollisionDetection>();
        }

        
    }

    private void OnEnable()
    {
        if (collisionDetection != null)
            collisionDetection.Event_Collision.Subscribe(Event_ArrowHit);
    }

    private void OnDisable()
    {
        if(collisionDetection != null)
            collisionDetection.Event_Collision.UnSubscribe(Event_ArrowHit);
    }

    protected void Event_ArrowHit(object sender, Collision args)
    {
        PlaySounds(sounds);
    }
}
