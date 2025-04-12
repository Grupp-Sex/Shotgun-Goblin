using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class TelekenesisPhysicsObject : MonoBehaviour, IFrozenOnFractionFreeze
{

    [SerializeField] public float BoundSize;
    [SerializeField] public Vector3 Velocity;
    [SerializeField] public Vector3 ForceSum;

    [SerializeField] public bool CanBeGrabbed;

    [SerializeField] IOnTelekenesisEnter[] onTelekenesisEnterScripts;
    [SerializeField] IOnTelekenesisLeave[] onTelekenesisLeaveScripts;

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
    

    // Start is called before the first frame update
    void Start()
    {


        CheckRigidBody();
        SaveSavedState();
        //InitializeConstraint();

        //Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        BoundSize = GetComponent<Collider>().bounds.size.magnitude;

        onTelekenesisEnterScripts = GetComponents<IOnTelekenesisEnter>();
        onTelekenesisLeaveScripts = GetComponents<IOnTelekenesisLeave>();
    }

    public void Update()
    {
        
        Velocity = Rigidbody.velocity;
        ForceSum = Rigidbody.GetAccumulatedForce();


    }

    



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
        SaveSavedState();
        NotifyTelekenesisEnter();
    }

    public void OnLeaveTelekenesis()
    {
        LoadSavedState();
        NotifyTelekenesisLeave();
        
    }

    protected void NotifyTelekenesisEnter()
    {
        for (int i = 0; i < onTelekenesisEnterScripts.Length; i++)
        {
            onTelekenesisEnterScripts[i].OnTelekenesisEnter();
        }
    }

    protected void NotifyTelekenesisLeave()
    {
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