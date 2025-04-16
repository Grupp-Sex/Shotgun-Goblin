using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectOnDeath : MonoBehaviour, IDeathActivated
{
    

    [SerializeField] List<GameObject> DroppedObjects = new List<GameObject>();

    public GameObject DeathDroppedObjects;


    private void Awake()
    {
        CreateDroppedObjectDestination();
    }

    protected GameObject CreateDroppedObjectDestination()
    {
        if (DeathDroppedObjects != null)
        {
            DeathDroppedObjects = Instantiate(new GameObject("DeathDroppedObjects"));
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
        

        obj.transform.parent = CreateDroppedObjectDestination().transform;
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
