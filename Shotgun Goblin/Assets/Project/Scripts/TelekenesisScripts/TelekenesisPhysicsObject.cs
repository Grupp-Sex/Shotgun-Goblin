using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekenesisPhysicsObject : MonoBehaviour
{
    public Rigidbody Rigidbody {  get; protected set; }
    protected void CheckRigidBody()
    {
        Rigidbody = GetComponent<Rigidbody>();

        if (Rigidbody == null)
        {
            Debug.Log("Missing Rigidbody in " + name + "!");
        }
    }

    // saves data that might be modifyed by telekenesis abilaties, in order to be able to reset it afterwards
    // (when it is no logner affected by telekenesis)
    protected SavedState savedState = new SavedState();


    // Start is called before the first frame update
    void Start()
    {
        CheckRigidBody();
        SaveSavedState();
    }

    public void OnEnterTeleknesis()
    {
        SaveSavedState();
    }

    public void OnLeaveTelekenesis()
    {
        LoadSavedState();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    protected void SaveSavedState()
    {
        savedState.useGravity = Rigidbody.useGravity;

        savedState.liearDrag = Rigidbody.drag;
        savedState.angularDrag = Rigidbody.angularDrag;

        savedState.maxAngularVelocity = Rigidbody.maxAngularVelocity;
        savedState.maxLinearVelocity = Rigidbody.maxLinearVelocity;

        savedState.isKinematic = Rigidbody.isKinematic;
    }
    protected void LoadSavedState()
    {
        Rigidbody.useGravity = savedState.useGravity;

        Rigidbody.drag = savedState.liearDrag;
        Rigidbody.angularDrag = savedState.angularDrag;

        Rigidbody.maxAngularVelocity = savedState.maxAngularVelocity;
        Rigidbody.maxLinearVelocity = savedState.maxLinearVelocity;

        Rigidbody.isKinematic = savedState.isKinematic;
    }

    protected struct SavedState
    {
        public bool useGravity;
        public bool isKinematic;


        public float liearDrag;
        public float angularDrag;

        public float maxLinearVelocity;
        public float maxAngularVelocity;

    }
}
