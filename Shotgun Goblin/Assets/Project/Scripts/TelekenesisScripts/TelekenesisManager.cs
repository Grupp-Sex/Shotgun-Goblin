using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TelekenesisManager : MonobehaviorScript_ToggleLog
{

    [SerializeField] public Transform TargetPosition { get; protected set; }
    [SerializeField] public float GrabDistanceThreshold { get; protected set; }
    [SerializeField] public float HoldDistanceThreshold { get; protected set; }
    [SerializeField] public List<TelekenesisPhysicsObject> HeldObjects { get; protected set; }


    public Vector3 GetTargetPosition => TargetPosition.position;

    public int GetHeldObjectCount => HeldObjects.Count;

    protected BaseTelekenesisAbilaty[] telekenesisAbilaties;
  


    void Start()
    {
        HeldObjects = new List<TelekenesisPhysicsObject>();

        InitialzieAllAbilaties();
    }

    

    protected virtual void AddHeldObject(TelekenesisPhysicsObject obj)
    {
        obj.OnEnterTeleknesis();
        HeldObjects.Add(obj);
        
    }

    protected virtual bool TryAddHeldObject(TelekenesisPhysicsObject obj)
    {
        bool containsObjectAlready = HeldObjects.Contains(obj);

        if (!containsObjectAlready)
        {
            AddHeldObject(obj);

        }
        else
        {
            Debug.Log("error in " + name + ":" + '\n' + "Failed to add object to held item list, Object already exists in list.");
        }

        return !containsObjectAlready;
    }

    protected virtual void RemoveHeldObject(TelekenesisPhysicsObject obj)
    {
        obj.OnLeaveTelekenesis();
        HeldObjects.Remove(obj);
    }




    #region Abilaties

    protected void InitialzieAllAbilaties()
    {
        telekenesisAbilaties = GetComponents<BaseTelekenesisAbilaty>();

        for (int i = 0; i < telekenesisAbilaties.Length; i++)
        {
            telekenesisAbilaties[i].Initialize(HeldObjects, TargetPosition);
        }
    }

    protected void NotifyGrabOject(TelekenesisPhysicsObject obj)
    {
        foreach (var abilaty in telekenesisAbilaties)
        {
            abilaty.OnGrabTelekenesisObject(obj);
        }
    }

    protected void NotifyThrowOject(TelekenesisPhysicsObject obj)
    {
        foreach (var abilaty in telekenesisAbilaties)
        {
            abilaty.OnThrowTelekenesisObject(obj);
        }
    }

    protected void NotifyDropOject(TelekenesisPhysicsObject obj)
    {
        foreach (var abilaty in telekenesisAbilaties)
        {
            abilaty.OnDroppTelekenesisObject(obj);
        }
    }

    #endregion


        


    #region Grab Objects    
    public void GrabOjcects()
    {
        TelekenesisPhysicsObject[] allOjects = GetAllAfectableObjects();

        for(int i = 0; i < allOjects.Length; i++)
        {
            if (IsPhysicsObjectWithinTheshold(allOjects[i], GrabDistanceThreshold))
            {
                GrabOneObject(allOjects[i]);
            }
        }

        DebugLog("telekenesis. grabed objects, held object count is: " + GetHeldObjectCount);

    }
    protected void GrabOneObject(TelekenesisPhysicsObject obj)
    {
        if (TryAddHeldObject(obj))
        {
            NotifyGrabOject(obj);
        }
        
    }


    #endregion



    #region Throw Objects
    public void ThrowObjects()
    {
        DebugLog("telekenesis. thrown objects, nr of objects thrown is: " + GetHeldObjectCount);

        for (int i = 0; i < HeldObjects.Count; i++)
        {
            ThrowOneObject(HeldObjects[i]);
            i--;
        }
    }
    protected void ThrowOneObject(TelekenesisPhysicsObject obj)
    {
        RemoveHeldObject(obj);
        NotifyThrowOject(obj);
    }
    

    #endregion



    #region Drop Objects
    protected void CheckObjectsForDropping()
    {
        for(int i = 0; i < HeldObjects.Count; i++)
        {
            if (!IsPhysicsObjectWithinTheshold(HeldObjects[i], HoldDistanceThreshold))
            {
                DropOneObject(HeldObjects[i]);
                i--;
            }
        }
    }
    public void DropAllObjects()
    {
        for(int i = 0;i < HeldObjects.Count; i++)
        {
            DropOneObject(HeldObjects[i]);
        }
    }
    protected void DropOneObject(TelekenesisPhysicsObject obj)
    {
        RemoveHeldObject(obj);
        DebugLog("dropped one object, object count is now: " + GetHeldObjectCount);

        NotifyDropOject(obj);

        
    }


    #endregion


    private void FixedUpdate()
    {
        CheckObjectsForDropping();
    }





    #region other stuff

    // get all objects that can be affected by telekenesis
    public TelekenesisPhysicsObject[] GetAllAfectableObjects()
    {
        return FindObjectsOfType<TelekenesisPhysicsObject>();
    }


    protected bool IsPhysicsObjectWithinTheshold(TelekenesisPhysicsObject obj, float threshold)
    {
        return Vector3DistanceSquared(GetTargetPosition, obj.transform.position) <= threshold * threshold;

    }


    public static float Vector3DistanceSquared(Vector3 a, Vector3 b)
    {
        Vector3 ab = a - b;

        return Vector3.SqrMagnitude(ab);
    }
    #endregion
}
