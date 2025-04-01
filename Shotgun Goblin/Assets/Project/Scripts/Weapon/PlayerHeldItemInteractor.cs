using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHeldItemInteractor : MonoBehaviour, IDebugActivatableVoid
{
    // most likely going to be used for guns and not mutch more, but still. Generalized for futureproffing purpoces.

    // the parrent gameobject to the item (gun)
    // Rather than the item (gun) itself being referenced here, the hand is.
    // This is done to facilitate swapign items (guns) without having to tell this script.
    [SerializeField] GameObject ItemHolder;

    void Start()
    {
        
    }


    // extracts the interface that tells the item to do an acction from each of the ItemHolders children
    private IHeldItem[] GetHeldItems()
    {
        return ItemHolder.GetComponentsInChildren<IHeldItem>();
    }

    // tells the item to activate its interation function. shooting function for guns.
    private void ItemIteract(IHeldItem item)
    {
        item.DoAction();

    }


    public void HeldItemDoInteraction()
    {

        
        IHeldItem[] items = GetHeldItems();
        Debug.Log("Items activated " + items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            ItemIteract(items[i]);
        }

        
    }


    public void VoidDebugActivate()
    {
        HeldItemDoInteraction();
    }

}

public interface IHeldItem
{
    public void DoAction();

}
