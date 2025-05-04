using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnIfNoChildren : MonoBehaviour
{
    public ObjectDespawner despawner;

    [SerializeField] float despawnTimer;

    protected int childCount;



    void Start()
    {
        if(despawner == null)
        {
            despawner = GetComponent<ObjectDespawner>();    
        }

        GameObject[] children = transform.GetComponentsInChildren<GameObject>();

        childCount = children.Length;

        for(int i = 0; i < childCount; i++)
        {
            EventOnDestroy disableEvent = children[i].AddComponent<EventOnDestroy>();


            disableEvent.Event_OnDisable.Subscribe(Event_ChildDisabled);
        }
    }

    protected void Event_ChildDisabled(object sender, object args)
    {
        childCount--;
        if(childCount == 0)
        {
            despawner.StartDespawnTimer(this, despawnTimer);
        }
    }

    
}
