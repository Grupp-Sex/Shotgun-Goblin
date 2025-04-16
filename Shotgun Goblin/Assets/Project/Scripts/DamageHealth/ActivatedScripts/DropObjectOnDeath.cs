using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectOnDeath : MonoBehaviour, IDeathActivated
{
    

    [SerializeField] List<GameObject> DroppedObjects = new List<GameObject>();

    static protected GameObject DeathDroppedObjects_Static;
    [SerializeField] GameObject DeathDroppedObjects;

    private void Awake()
    {
        CreateDroppedObjectDestination();
    }

    protected GameObject CreateDroppedObjectDestination()
    {
        if(DeathDroppedObjects_Static == null)
        {
            DeathDroppedObjects_Static = new GameObject("DeathDroppedObjects");
        }
        if (DeathDroppedObjects == null)
        {
           
            DeathDroppedObjects = DeathDroppedObjects_Static;
        }

        return DeathDroppedObjects;
    }

    public void OnDeath(DamageInfo damageInfo)
    {
        for(int i = 0; i < DroppedObjects.Count; i++)
        {
            DroppObject(DroppedObjects[i]);
        }
    }

    protected void DroppObject(GameObject obj)
    {
        if(obj == null) return;

        CreateDroppedObjectDestination();
        obj.transform.parent = DeathDroppedObjects_Static.transform;
        ActivateObjectDroppScripts(obj);
        
    }

    protected void ActivateObjectDroppScripts(GameObject obj)
    {
        IOnDropped[] onDroppedOnDeathScripts = obj.GetComponents<IOnDropped>();

        for (int i = 0; i < onDroppedOnDeathScripts.Length; i++)
        {
            onDroppedOnDeathScripts[i].OnDropped();
        }
    }
}

public interface IOnDropped
{
    public void OnDropped();
}
