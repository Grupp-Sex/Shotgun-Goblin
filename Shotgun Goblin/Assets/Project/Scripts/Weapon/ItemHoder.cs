using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoder : MonoBehaviour
{
    [SerializeField] List<GameObject> heldItems;

    // can be subscribed to by other scripts to be run whenever the item is changed
    public event Action<ItemHoder> Event_StateChanged;

    private void OnEnable()
    {
        StateChanged();
    }

    private void OnValidate()
    {
        StateChanged();   
    }

    protected void StateChanged()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            
            Event_StateChanged?.Invoke(this);
            
        }
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

            for(int j = 0; j < objectHeldItems.Length; j++)
            {
                interaction.Invoke(objectHeldItems[j]);
            }
        }


    }

    
}
