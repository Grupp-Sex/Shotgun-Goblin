using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTelekenesisAbilaty : MonobehaviorScript_ToggleLog
{
    protected List<TelekenesisPhysicsObject> heldObjects;

    protected Transform pickuppOriginPoint;
    protected TelekenesisManager parentManager;

    protected Rigidbody parentRB;

    public virtual void Initialize(TelekenesisManager parentManager, List<TelekenesisPhysicsObject> heldObjects, Transform origin)
    {
        this.heldObjects = heldObjects;
        this.parentManager = parentManager;
        pickuppOriginPoint = origin;

        parentRB = parentManager.gameObject.GetComponent<Rigidbody>();

        DebugLog("Initialized telekenesis abilaty");
    }

    public virtual void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec){}
    public virtual void OnThrowTelekenesisObject(TelekenesisPhysicsObject thrownObject){}

    public virtual void OnDroppTelekenesisObject(TelekenesisPhysicsObject droppedObject){}

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

    protected Vector3 Direction(Vector3 a, Vector3 b)
    {
        return a - b;
    }

    protected Vector3 NormalDirection(Vector3 a, Vector3 b)
    {
        return (a - b).normalized;
    }


}
