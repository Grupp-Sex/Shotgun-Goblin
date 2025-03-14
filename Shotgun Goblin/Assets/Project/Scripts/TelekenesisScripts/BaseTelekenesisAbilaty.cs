using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTelekenesisAbilaty : MonobehaviorScript_ToggleLog
{
    protected List<TelekenesisPhysicsObject> heldObjects;

    protected Transform pickuppOriginPoint; 

    public virtual void Initialize(List<TelekenesisPhysicsObject> heldObjects, Transform origin)
    {
        this.heldObjects = heldObjects;
        pickuppOriginPoint = origin;

        DebugLog("Initialized telekenesis abilaty");
    }

    public virtual void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec){}
    public void OnThrowTelekenesisObject(TelekenesisPhysicsObject thrownObject){}

    public void OnDroppTelekenesisObject(TelekenesisPhysicsObject droppedObject){}

    protected virtual void RunFuncOnAllObjects(List<TelekenesisPhysicsObject> objects, Action<TelekenesisPhysicsObject> func)
    {
        for(int i = 0; i < objects.Count; i++)
        {
            func.Invoke(objects[i]);
        }
    }
    protected virtual void RunFuncOnAllHeldObjects(Action<TelekenesisPhysicsObject> func)
    {
        RunFuncOnAllObjects(heldObjects, func);
    }

    protected virtual void OnUpdate()
    {

    }
        
    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected Vector3 Direction(GameObject a, GameObject b)
    {
        return a.transform.position - b.transform.position;
    }

    protected Vector3 NormalDirection(GameObject a, GameObject b)
    {
        return (a.transform.position - b.transform.position).normalized;
    }


}
