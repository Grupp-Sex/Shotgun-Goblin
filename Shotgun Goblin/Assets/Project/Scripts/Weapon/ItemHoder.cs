using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ansgar
public class ItemHoder : MonobehaviorScript_ToggleLog
{
    [SerializeField] List<GameObject> heldItems;

    // can be subscribed to by other scripts to be run whenever the item is changed
    public event Action<ItemHoder> Event_StateChanged;

    private void Start()
    {
        StateChanged(true);
    }

    private void OnEnable()
    {
        StateChanged();
    }

    private void OnValidate()
    {
        StateChanged();   
    }

    protected void StateChanged(bool overide)
    {
        if ((isActiveAndEnabled && Application.isPlaying) || overide)
        {
            
            Event_StateChanged?.Invoke(this);
            
        }
    }

    protected void StateChanged()
    {
        StateChanged(false);
    }

    public void AddItem(GameObject item)
    {
        if(!heldItems.Contains(item))
        {
            heldItems.Add(item);
            StateChanged();
        }
        
    }

    public void RemoveItem(GameObject item)
    {
        if (heldItems.Contains(item))
        {
            heldItems.Remove(item);
            StateChanged();
        }
    }

    public void InteractWithAllHeldItems<T>(Action<T> interaction)
    {
        
        for(int i = 0; i < heldItems.Count; i++)
        {
            T[] objectHeldItems = heldItems[i].GetComponents<T>();

            DebugLog(objectHeldItems.Length + " components found of type: " + typeof(T).Name);

            for(int j = 0; j < objectHeldItems.Length; j++)
            {
                interaction.Invoke(objectHeldItems[j]);
            }
        }


    }

    
}
