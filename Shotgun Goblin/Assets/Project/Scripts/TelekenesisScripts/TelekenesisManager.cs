using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TelekenesisManager : MonobehaviorScript_ToggleLog
{
    [SerializeField] public bool DropOriginPoint;
    [SerializeField] public Transform TargetPosition;
    [SerializeField] public GameObject AbilatiyScriptHolder;
    

    [Header("Held Items")]
    [SerializeField] public float GrabDistanceThreshold;
    [SerializeField] public float HoldDistanceThreshold;
    [SerializeField] public int HeldObjectMax;
    public float MaxObjectBoundSize = 4;

    [Header("Cooldown And Duration")]
    [SerializeField] float CooldownTimer = 2;
    [SerializeField] bool DoDuration = true;
    [SerializeField] float DurationTimer = 5;
    [SerializeField] bool OnCooldown;
    [SerializeField] bool CanGrab = true;

    public bool IsActivated { get; protected set; }


    [Header("Held Objects")]
    [SerializeField] public List<TelekenesisPhysicsObject> HeldObjects;

    


    public Vector3 GetTargetPosition => TargetPosition.position;

    public int GetHeldObjectCount => HeldObjects.Count;

    protected BaseTelekenesisAbilaty[] telekenesisAbilaties;
    protected IOnTelekenesis[] telekenesisVisuals;
    protected IOnTelekenesisThrow[] telekenesisThrowVisuals;


    void Start()
    {
        if (AbilatiyScriptHolder == null)
        {
            AbilatiyScriptHolder = gameObject;
        }

        if (DropOriginPoint)
        {
            TargetPosition.SetParent(null);
        }

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

        if (!containsObjectAlready && obj.CanBeGrabbed)
        {
            AddHeldObject(obj);
            return true;
        }
        else if(containsObjectAlready)
        {
            Debug.Log("error in " + name + ":" + '\n' + "Failed to add object to held item list, Object already exists in list.");
        }

        return false;
    }

    protected virtual void RemoveHeldObject(TelekenesisPhysicsObject obj)
    {
        obj.OnLeaveTelekenesis();
        HeldObjects.Remove(obj);
    }

    #region Cooldown And Activate
    protected void StartTelekeneis()
    {
        IsActivated = true;
        NotifyTelekenesisStart();
    }

    protected void EndTelekeneis()
    {
        IsActivated = false;

        EndDuration();
        NotifyTelekenesisEnd();
        DropAllObjects();
    }
    
    public void ActivateGrab()
    {
        if (!IsActivated)
        {
            if (CanGrab)
            {
                StartTelekeneis();
                GrabOjcects();
                StartCooldown();
                StartDuration();
            }
        }
    }

    public void ActivateThrow()
    {
        if (IsActivated)
        {
            
            ThrowObjects();
            NotifyTelekeneisisThrow();
            EndTelekeneis();
        }
    }


    protected void StartCooldown()
    {
        if (!OnCooldown)
        {
            CooldownCoroutine = DoCooldownTimer(CooldownTimer);
            StartCoroutine(CooldownCoroutine);
        }
    }

    protected void EndCooldown()
    {
        if (CooldownCoroutine != null)
        {
            
            StopCoroutine(CooldownCoroutine);
            CooldownCoroutine = null;

            CanGrab = true;
            OnCooldown = false;
        }
    }

    protected IEnumerator CooldownCoroutine;
    protected IEnumerator DoCooldownTimer(float time)
    {
        OnCooldown = true;
        CanGrab = false;
        yield return new WaitForSeconds(time);  
        CanGrab = true;

        OnCooldown = false;
    }

    

    protected void StartDuration()
    {
        DurationCoroutine = DoDurationTimer(DurationTimer);
        StartCoroutine(DurationCoroutine);
    }

    protected void EndDuration()
    {
        if (DurationCoroutine != null)
        {
            StopCoroutine(DurationCoroutine);
            DurationCoroutine = null;
        }
    }

    protected IEnumerator DurationCoroutine;

    protected IEnumerator DoDurationTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if(IsActivated)
        {
            ActivateThrow();
        }
    }

    protected void AbortTelekenesis()
    {
        DebugLog("Telekenesis Aborted");
        EndDuration();
        EndCooldown();
        EndTelekeneis();
    }


    #endregion


    #region Abilaties

    protected void InitialzieAllAbilaties()
    {
        telekenesisAbilaties = AbilatiyScriptHolder.GetComponents<BaseTelekenesisAbilaty>();

        telekenesisVisuals = AbilatiyScriptHolder.GetComponents<IOnTelekenesis>();
        telekenesisThrowVisuals = AbilatiyScriptHolder.GetComponents<IOnTelekenesisThrow>();

        for (int i = 0; i < telekenesisAbilaties.Length; i++)
        {
            telekenesisAbilaties[i].Initialize(this, HeldObjects, TargetPosition);
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

    protected void NotifyTelekenesisStart()
    {
        foreach (var visual in telekenesisVisuals)
        {
            visual.OnTelekenesisStart();
        }
    }

    protected void NotifyTelekenesisEnd()
    {
        foreach (var visual in telekenesisVisuals)
        {
            visual.OnTelekenesisEnd();
        }
    }

    protected void NotifyTelekeneisisThrow()
    {
        foreach (var throwVisual in telekenesisThrowVisuals)
        {
            throwVisual.OnTelekenesisThrow();
        }
    }

    #endregion


        


    #region Grab Objects    
    public void GrabOjcects()
    {
        GrabOjcects(GetTargetPosition, GrabDistanceThreshold);

    }

    protected void GrabOjcects(Vector3 position, float range)
    {
        Collider[] colliders = Physics.OverlapSphere(position, range);

        SortedList<SortableWrapper<double>, TelekenesisPhysicsObject> grabbedList = new SortedList<SortableWrapper<double>, TelekenesisPhysicsObject>();
        
        for (int i = 0; i < colliders.Length; i++)
        {
            TelekenesisPhysicsObject newObject = colliders[i].GetComponent<TelekenesisPhysicsObject>();

            if( newObject != null && newObject.isActiveAndEnabled && newObject.CanBeGrabbed && CheckObjectSize(newObject, MaxObjectBoundSize))
            {
                Vector3 ClosestPoint = colliders[i].ClosestPoint(position);

                float distance = Vector3.Distance(ClosestPoint, position) / range;

                double priority = newObject.GetPickuppPriority(distance, i);

                DebugLog("Object Priority: " + priority);

                grabbedList.Add(SortableWrapper<double>.Create(priority), newObject);
            }
        }

        int heldObjectCount = 0;

        if (HeldObjectMax > 0)
        {
            foreach (var collisionObject in grabbedList)
            {
                if (heldObjectCount >= HeldObjectMax) break;
                heldObjectCount++;
                GrabOneObject(collisionObject.Value);
                
            }
        }
        
        DebugLog("telekenesis. grabed objects, held object count is: " + GetHeldObjectCount);

    }

    protected bool CheckObjectSize(TelekenesisPhysicsObject obj, float threshold)
    {

        return !(obj.Bounds.x > threshold || obj.Bounds.y > threshold || obj.Bounds.z > threshold);
    }


    protected void LegacyGrabObjects()
    {
        TelekenesisPhysicsObject[] allOjects = GetAllAfectableObjects();

        for (int i = 0; i < allOjects.Length; i++)
        {
            if (IsPhysicsObjectWithinTheshold(allOjects[i], GrabDistanceThreshold))
            {
                if (GetHeldObjectCount < HeldObjectMax)
                {
                    GrabOneObject(allOjects[i]);

                }
                else
                {
                    DebugLog("telekenesis, max amount of held objects reached");
                    break;
                }
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
            if (HeldObjects[i] != null)
            {

                if (!IsPhysicsObjectWithinTheshold(HeldObjects[i], HoldDistanceThreshold))
                {
                    DropOneObject(HeldObjects[i]);
                    i--;
                }
            }
            else
            {
                HeldObjects.RemoveAt(i);
                i--;    

            }
        }

        if (IsActivated)
        {
            if (HeldObjects.Count <= 0)
            {
                AbortTelekenesis();
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


    private void Update()
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
        return Vector3DistanceSquared(GetTargetPosition, obj.BoundCollider.bounds.center) <= threshold * threshold;

    }

    


    public static float Vector3DistanceSquared(Vector3 a, Vector3 b)
    {
        Vector3 ab = a - b;

        return Vector3.SqrMagnitude(ab);
    }
    #endregion

    protected class SortableWrapper<T>: IComparable<SortableWrapper<T>> where T : IComparable
    {
        public T Value;
        public static SortableWrapper<T> Create(T value)
        {
            
            return new SortableWrapper<T>() { Value = value};
        }

        public int CompareTo(SortableWrapper<T> obj)
        {
            int result = Value.CompareTo(obj.Value);

            if(result == 0)
            {
                result = 1;
            }

            return result;
        }

    }
}

public interface IOnTelekenesis
{
    public void OnTelekenesisStart();
    public void OnTelekenesisEnd();
}

public interface IOnTelekenesisThrow
{
    public void OnTelekenesisThrow();
}
