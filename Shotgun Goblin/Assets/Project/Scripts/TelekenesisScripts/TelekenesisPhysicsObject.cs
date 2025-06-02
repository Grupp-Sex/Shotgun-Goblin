using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class TelekenesisPhysicsObject : MonobehaviorScript_ToggleLog, IFrozenOnFractionFreeze
{

    public Collider BoundCollider;

    [SerializeField] public float BoundSize;
    public Vector3 Bounds;
    //[SerializeField] public Vector3 Velocity;
    //[SerializeField] public Vector3 ForceSum;

    [SerializeField] public bool CanBeGrabbed;

    [SerializeField] IOnTelekenesisEnter[] onTelekenesisEnterScripts;
    [SerializeField] IOnTelekenesisLeave[] onTelekenesisLeaveScripts;

    [SerializeField] byte PriorityLayer;
    [SerializeField] byte PickupPriority;

    protected List<Func<int, int>> PickuppModifications = new List<Func<int, int>>();

    public EventPusher<TelekenesisPhysicsObject> Event_OnTelekenesisEnter { get; private set; } = new EventPusher<TelekenesisPhysicsObject>();
    public EventPusher<TelekenesisPhysicsObject> Event_OnTelekenesisLeave { get; private set; } = new EventPusher<TelekenesisPhysicsObject>();

    private void OnValidate()
    {
        if (isActiveAndEnabled)
        {
            ClampPriority();
        }
    }



    public bool IsFrozen { get; set; }
    
    public Rigidbody Rigidbody {  get; protected set; }

    // saves data that might be modifyed by telekenesis abilaties, in order to be able to reset it afterwards
    // (when it is no logner affected by telekenesis)
    protected SavedState savedState = new SavedState();


    public void Freze()
    {
        IsFrozen = true;
        CanBeGrabbed = false;
    }

    public void Thaw()
    {
        IsFrozen = false;
        CanBeGrabbed = true;
        
    }
    
    protected void ClampPriority()
    {
        PriorityLayer = Math.Clamp(PriorityLayer, (byte)0, (byte)10);
        PickupPriority = Math.Clamp(PickupPriority, (byte)0, (byte)10);

    }
    public double GetPickuppPriority(float distance, int counter)
    {

        ClampPriority();

        int pickupLayer = PriorityLayer * 1000; // 10___ - 00____

        int subPriority = PickupPriority; // 10_ - 00_

        float modDistance = distance;

        int subPriorityMod = subPriority;

        for (int i = 0; i < PickuppModifications.Count; i++)
        {

            subPriorityMod = PickuppModifications[i].Invoke(subPriority);

            math.clamp(subPriorityMod, 1, 0);

            subPriority = (byte)subPriorityMod;
        }

        math.clamp(modDistance, 1, 0);

        modDistance = 1 - modDistance;


       

        

        double sum = pickupLayer + subPriority * 10 + modDistance;

        DebugLog("Telekenesis Pickupp priority: Layer:" + PriorityLayer + ", Sub Layer " + subPriority + " distance: " + distance + ", sum: " + sum);

        

        return -sum;
    }

    public void AddPickuppModification()
    {

    }

    public void RemovePickuppModification()
    {

    }

    // Start is called before the first frame update
    void Start()
    {


        CheckRigidBody();
        SaveSavedState();
        //InitializeConstraint();

        //Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        if(BoundCollider == null)
        {
            BoundCollider = GetComponent<Collider>();
        }
        Bounds = BoundCollider.bounds.size;

        BoundSize = BoundCollider.bounds.size.magnitude;

        onTelekenesisEnterScripts = GetComponents<IOnTelekenesisEnter>();
        onTelekenesisLeaveScripts = GetComponents<IOnTelekenesisLeave>();
    }

    //public void Update()
    //{
        
    //    Velocity = Rigidbody.velocity;
    //    ForceSum = Rigidbody.GetAccumulatedForce();


    //}

    



    protected void CheckRigidBody()
    {
        Rigidbody = GetComponent<Rigidbody>();

        if (Rigidbody == null)
        {
            Debug.Log("Missing Rigidbody in " + name + "!");
            gameObject.AddComponent<Rigidbody>();
        }
    }


    

    public void OnEnterTeleknesis()
    {
        if (isActiveAndEnabled)
        {
            SaveSavedState();
            NotifyTelekenesisEnter();
        }
    }

    public void OnLeaveTelekenesis()
    {
        if (isActiveAndEnabled)
        {
            LoadSavedState();
            NotifyTelekenesisLeave();
        }
        
    }

    protected void NotifyTelekenesisEnter()
    {
        Event_OnTelekenesisEnter.Invoke(this, this);

        for (int i = 0; i < onTelekenesisEnterScripts.Length; i++)
        {
            onTelekenesisEnterScripts[i].OnTelekenesisEnter();
        }
    }

    protected void NotifyTelekenesisLeave()
    {
        Event_OnTelekenesisLeave.Invoke(this, this);

        for (int i = 0; i < onTelekenesisLeaveScripts.Length; i++)
        {
            onTelekenesisLeaveScripts[i].OnTelekenesisLeave();
        }
    }

    public void SetNewParrent(Transform parrent)
    {
        transform.SetParent(parrent);
    }

    protected void SaveSavedState()
    {
        savedState.parrent = transform.parent;

        savedState.useGravity = Rigidbody.useGravity;

        savedState.liearDrag = Rigidbody.drag;
        savedState.angularDrag = Rigidbody.angularDrag;

        savedState.maxAngularVelocity = Rigidbody.maxAngularVelocity;
        savedState.maxLinearVelocity = Rigidbody.maxLinearVelocity;

        savedState.isKinematic = Rigidbody.isKinematic;
    }
    protected void LoadSavedState()
    {
        

        transform.SetParent(savedState.parrent, true);

        Rigidbody.useGravity = savedState.useGravity;

        Rigidbody.drag = savedState.liearDrag;
        Rigidbody.angularDrag = savedState.angularDrag;

        Rigidbody.maxAngularVelocity = savedState.maxAngularVelocity;
        Rigidbody.maxLinearVelocity = savedState.maxLinearVelocity;

        Rigidbody.isKinematic = savedState.isKinematic;
    }

    protected struct SavedState
    {
        public Transform parrent;

        public bool useGravity;
        public bool isKinematic;


        public float liearDrag;
        public float angularDrag;

        public float maxLinearVelocity;
        public float maxAngularVelocity;

    }
}

public interface IOnTelekenesisEnter
{
    public void OnTelekenesisEnter();

}

public interface IOnTelekenesisLeave
{
    public void OnTelekenesisLeave();

}