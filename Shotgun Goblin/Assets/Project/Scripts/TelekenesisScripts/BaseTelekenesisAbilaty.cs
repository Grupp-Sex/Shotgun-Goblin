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

    public virtual void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec)
    {

    }
    public void OnThrowTelekenesisObject(TelekenesisPhysicsObject thrownObject)
    {

    }

    public void OnDroppTelekenesisObject(TelekenesisPhysicsObject droppedObject)
    {

    }

    protected virtual void RunFuncOnAllObjects<T>(List<TelekenesisPhysicsObject> objects, Action<TelekenesisPhysicsObject> func)
    {
        for(int i = 0; i < objects.Count; i++)
        {
            func.Invoke(objects[i]);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
