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
    [SerializeField] ItemHoder itemHolder;

    

    void Start()
    {
        
    }

    private void OnDisable()
    {
        if (itemHolder != null)
        {
            itemHolder.Event_StateChanged -= OnItemHolderStateChange;
        }
    }

    private void OnEnable()
    {
        if (itemHolder != null)
        {
            itemHolder.Event_StateChanged += OnItemHolderStateChange;
        }
    }

    protected void OnItemHolderStateChange(ItemHoder newObjects)
    {
        itemHolder.InteractWithAllHeldItems<IUserReference>(SetItemParrentReference);
    }
    
    protected void SetItemParrentReference(IUserReference item)
    {
        item.SetUser(gameObject);
    }

    // tells the item to activate its interation function. shooting function for guns.
    private void ItemIteract(IHeldItem item)
    {
        item.DoAction();

    }


    public void HeldItemDoInteraction()
    {
        itemHolder?.InteractWithAllHeldItems<IHeldItem>(ItemIteract);           
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

public interface IUserReference
{
    public void SetUser(GameObject user);

}
