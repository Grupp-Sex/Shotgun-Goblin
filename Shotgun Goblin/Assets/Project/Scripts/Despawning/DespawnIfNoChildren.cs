using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnIfNoChildren : MonoBehaviour
{
    public ObjectDespawner despawner;

    [SerializeField] float despawnTimer;

    public int childCount;

    protected bool firstFrame = true;
    

    private void Update()
    {
        if (firstFrame)
        {
            Initialize();

            firstFrame = false;
        }
        
    }

    


    protected void Initialize()
    {
        if (despawner == null)
        {
            despawner = GetComponent<ObjectDespawner>();
        }

        Transform[] children = transform.GetComponentsInChildren<Transform>();

        childCount = children.Length;

        for (int i = 0; i < childCount; i++)
        {
            EventOnDestroy disableEvent = children[i].gameObject.AddComponent<EventOnDestroy>();


            disableEvent.Event_OnDisable.Subscribe(Event_ChildDisabled);
        }
    }

    protected void Event_ChildDisabled(object sender, object args)
    {
        childCount--;
        if(childCount == 1)
        {
            despawner.StartDespawnTimer(this, despawnTimer);
        }
    }

    
    
}
